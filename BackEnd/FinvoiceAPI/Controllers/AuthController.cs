using BAL.Models;
using BAL.Services.Interfaces;
using BAL.Utils;
using BAL.Validators;
using DAL.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace FinvoiceAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly string _secretKey;
        private readonly IRoleService _roleService;
        private readonly IRefreshTokenService _refreshTokenService;

        public AuthController(IAccountService accountService, IConfiguration configuration, IRoleService roleService, IRefreshTokenService refreshTokenService)
        {
            _accountService = accountService;
            _secretKey = configuration.GetSection("AppSettings:SerectKey").Value;
            _roleService = roleService;
            _refreshTokenService = refreshTokenService;
        }

        #region Authentication & Authorization
        private async Task<TokenModel> GenerateToken(Account accountViewModel)
        {
            accountViewModel.Role = await _roleService.GetRole(new Role()
            {
                IdRole = accountViewModel.UserRole
            }, "GetRoleID");

            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, accountViewModel.FullName),
                new Claim(ClaimTypes.Email, accountViewModel.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserID", accountViewModel.IdAccount),
                new Claim(ClaimTypes.Role, accountViewModel.Role.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var signingCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signingCredential
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = FunctionRandom.GenerateRefreshToken();
            var refreshTokenEntity = new RefreshToken
            {
                Id = Guid.NewGuid(),
                IdAccount = accountViewModel.IdAccount,
                JwtId = token.Id,
                Token = refreshToken,
                IsUsed = false,
                IsRevoked = false,
                IssuedAt = DateTime.Now,
                ExpiredAt = DateTime.Now.AddHours(1)
            };

            await _refreshTokenService.AddRefreshToken(refreshTokenEntity);

            return new TokenModel
            {
                AccessToken = jwt,
                RefreshToken = refreshToken
            };
        }
        #endregion

        #region Login
        /// <summary>
        /// UC0-001
        /// Log into system using email and password
        /// </summary>    
        /// <remarks>
        ///     Sample request:
        ///
        ///         {
        ///           "email": "admin@gmail.com",
        ///           "password": "admin123"
        ///         }
        ///         
        /// </remarks>
        /// <returns>Specific HTTP Status code</returns>
        /// <response code="200">Return home screen if the access is successful</response>
        /// <response code="400">If the account is null</response>
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
        {
            string errorMessage = "";
            bool status = false;
            LoginValidator loginValidator = new();
            Account existingAccount;
            TokenModel? jwt = null;

            try
            {
                if (loginViewModel == null)
                {
                    throw new Exception("Api's information has been corrupted, please try again or contact developer for more support");
                }

                var validation = loginValidator.Validate(loginViewModel);

                if (!validation.IsValid)
                {
                    var errors = new List<string>();

                    foreach (var error in validation.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }

                    return BadRequest(new
                    {
                        Status = status,
                        Errors = errors
                    });
                }

                Account account = new()
                {
                    Email = loginViewModel.Email,
                    Password = loginViewModel.Password
                };

                existingAccount = await _accountService.GetAccount(account, "Login");

                if (existingAccount.Email == null)
                {
                    errorMessage = "Account does not exist!";

                    return BadRequest(new
                    {
                        Status = status,
                        ErrorMessage = errorMessage
                    });
                }
                else
                {
                    bool isValidPassword = BCrypt.Net.BCrypt.Verify(account.Password, existingAccount.Password);

                    if (isValidPassword == false)
                    {
                        errorMessage = "Wrong password, please try again!";

                        return BadRequest(new
                        {
                            Status = status,
                            ErrorMessage = errorMessage
                        });
                    }
                    else
                    {
                        if (existingAccount.Status == 0)
                        {
                            errorMessage = "Account not activated!";

                            return BadRequest(new
                            {
                                Status = status,
                                ErrorMessage = errorMessage
                            });
                        }
                        else if (existingAccount.Status == 2)
                        {
                            errorMessage = "Your account has been locked!";

                            return BadRequest(new
                            {
                                Status = status,
                                ErrorMessage = errorMessage
                            });
                        }
                        else if (existingAccount.Status == 3)
                        {
                            errorMessage = "Your account has been deleted!";

                            return BadRequest(new
                            {
                                Status = status,
                                ErrorMessage = errorMessage
                            });
                        }
                    }
                }

                jwt = await GenerateToken(existingAccount);
                status = true;
            }
            catch (Exception)
            {
                errorMessage = "Something went wrong!";
            }

            return Ok(new
            {
                Data = jwt,
                Status = status,
                ErrorMessage = errorMessage
            });
        }
        #endregion

        #region Sign Up
        /// <summary>
        /// UC0-002
        /// Sign Up account using full name, email, password and confirm password
        /// </summary>    
        /// <remarks>
        ///     Sample request:
        ///
        ///         {
        ///           "fullName": "ABC",
        ///           "email": "admin@gmail.com",
        ///           "password": "admin123",
        ///           "confirmPassword": "admin123"
        ///         }
        ///         
        /// </remarks>
        /// <returns>Specific HTTP Status code</returns>
        /// <response code="200">Return home screen if the access is successful</response>
        /// <response code="400">If the account is null</response>
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignUpAccount([FromBody] SignUpViewModel signUpViewModel)
        {
            string errorMessage, userID = FunctionRandom.RandomCode(21);
            bool status = false;
            SignUpValidator signUpValidator = new();
            Role role;

            try
            {
                if (signUpViewModel == null)
                {
                    throw new Exception("Api's information has been corrupted, please try again or contact developer for more support");
                }

                var validation = signUpValidator.Validate(signUpViewModel);

                if (!validation.IsValid)
                {
                    var errors = new List<string>();

                    foreach (var error in validation.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }

                    return BadRequest(new
                    {
                        Status = status,
                        Errors = errors
                    });
                }

                role = await _roleService.GetRole(new Role()
                {
                    Name = "User"
                }, "GetRoleName");

                Account account = new()
                {
                    IdAccount = userID,
                    FullName = signUpViewModel.FullName,
                    Email = signUpViewModel.Email,
                    Password = signUpViewModel.Password,
                    TaxCode = FunctionRandom.RandomTaxCode(13),
                    ColorAvatar = FunctionRandom.ColorAvatar(),
                    DefaultAvatar = signUpViewModel.FullName.FirstOrDefault(),
                    UserRole = role.IdRole,
                    Status = 0
                };

                bool checkAddAccount = await _accountService.ActionAccount(account, "AddAccount");

                if (checkAddAccount == false)
                {
                    errorMessage = "Account already exists!";

                    return BadRequest(new
                    {
                        Status = status,
                        ErrorMessage = errorMessage
                    });
                }
                else
                {
                    bool checkSendEmail = await _accountService.ActionAccount(account, "SendOTP");

                    if (checkSendEmail == false)
                    {
                        errorMessage = "System error please try again!";

                        return BadRequest(new
                        {
                            Status = status,
                            ErrorMessage = errorMessage
                        });
                    }

                    errorMessage = "Successful account registration!";
                    status = true;

                    return Ok(new
                    {
                        Status = status,
                        ErrorMessage = errorMessage,
                    });
                }
            }
            catch (Exception)
            {
                errorMessage = "Something went wrong!";
            }

            return BadRequest(new
            {
                Status = status,
                ErrorMessage = errorMessage
            });
        }
        #endregion

        #region Send OTP
        /// <summary>
        /// UC0-003
        /// Input user's email in order to reset password
        /// </summary>
        /// <param name="email">Enter email to reset password</param>
        /// 
        /// <remarks>
        ///     Sample request:
        ///
        ///         "email": "example@gmail.com"
        ///             
        /// </remarks>
        /// <returns>Specific HTTP Status code</returns>
        /// <response code="200">Return login page</response>
        /// <response code="400">Confirm password not matched</response>
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendOTP([FromForm] string email)
        {
            string errorMessage = "";
            bool status = false;

            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest(new
                    {
                        Status = status,
                        ErrorMessage = "Email address is required!"
                    });
                }
                else if (!Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                {
                    return BadRequest(new
                    {
                        Status = status,
                        ErrorMessage = "Invalid Email address!"
                    });
                }

                Account account = new()
                {
                    Email = email
                };

                status = await _accountService.ActionAccount(account, "SendOTP");

                if (status)
                {
                    return Ok(new
                    {
                        Status = status,
                        ErrorMessage = errorMessage,
                    });
                }
            }
            catch (Exception)
            {
                errorMessage = "Something went wrong!";
            }

            return BadRequest(new
            {
                Status = status,
                ErrorMessage = errorMessage
            });
        }
        #endregion

        #region Confirm OTP
        /// <summary>
        /// UC0-004
        /// Input OTP code to confirm
        /// </summary>
        /// <param name="confirmOTPViewModel">Enter otp code to confirm</param>
        /// 
        /// <remarks>
        ///     Sample request:
        ///
        ///         "otpCode": "abc123"
        ///             
        /// </remarks>
        /// <returns>Specific HTTP Status code</returns>
        /// <response code="200">Return login page</response>
        /// <response code="400">Confirm password not matched</response>
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConfirmOTP([FromBody] ConfirmOTPViewModel confirmOTPViewModel)
        {
            bool status = false;
            Account getAccount;
            ConfirmOTPValidator confirmOTPValidator = new();

            try
            {
                if (confirmOTPViewModel == null)
                {
                    throw new Exception("Api's information has been corrupted, please try again or contact developer for more support");
                }

                var validation = confirmOTPValidator.Validate(confirmOTPViewModel);

                if (!validation.IsValid)
                {
                    var errors = new List<string>();

                    foreach (var error in validation.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }

                    return BadRequest(new
                    {
                        Status = status,
                        Errors = errors
                    });
                }

                Account account = new()
                {
                    Email = confirmOTPViewModel.UserEmail
                };

                getAccount = await _accountService.GetAccount(account, "GetByEmail");

                if (getAccount != null)
                {
                    if (getAccount.OtpCode != null)
                    {
                        if (getAccount.OtpCode.Equals(confirmOTPViewModel.OtpCode))
                        {
                            if (getAccount.OtpCodeTimeOut < DateTime.Now)
                            {
                                return BadRequest(new
                                {
                                    Status = status,
                                    ErrorMessage = "Your verification code has expired!"
                                });
                            }
                            else
                            {
                                getAccount.Status = 1;

                                var checkUpdate = await _accountService.ActionAccount(getAccount, "ConfirmOTP");

                                status = true;

                                return Ok(new
                                {
                                    Status = status,
                                    ErrorMessage = "",
                                });
                            }
                        }
                        else
                        {
                            return BadRequest(new
                            {
                                Status = status,
                                ErrorMessage = "Your verification code does not match!"
                            });
                        }
                    }
                    else
                    {
                        return BadRequest(new
                        {
                            Status = status,
                            ErrorMessage = "Your verification code does not exist!"
                        });
                    }
                }
                else
                {
                    return BadRequest(new
                    {
                        Status = status,
                        ErrorMessage = "Something went wrong! Please try again!"
                    });
                }
            }
            catch (Exception)
            {
                return BadRequest(new
                {
                    Status = status,
                    ErrorMessage = "Something went wrong! Please try again!"
                });
            }
        }
        #endregion

        #region Reset Password
        /// <summary>
        /// UC0-005
        /// Enter new password to reset password
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///
        ///         {
        ///           "NewPassword": "123456",
        ///           "ConfirmPassword": "123456"
        ///         }
        ///                  
        /// </remarks>
        /// <returns>Specific HTTP Status code</returns>
        /// <response code="200">return comfirmPassword page</response>
        /// <response code="400">Wrong OTP</response>
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordViewModel resetPasswordViewModel)
        {
            string errorMessage = "";
            bool status = false;
            ResetPasswordValidator validator = new();

            try
            {
                if (resetPasswordViewModel == null)
                {
                    throw new Exception("Api's information has been corrupted, please try again or contact developer for more support");
                }

                var validation = validator.Validate(resetPasswordViewModel);

                if (!validation.IsValid)
                {
                    var errors = new List<string>();

                    foreach (var error in validation.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }

                    return BadRequest(new
                    {
                        Status = status,
                        Errors = errors
                    });
                }

                Account account = new()
                {
                    Email = resetPasswordViewModel.UserEmail,
                    Password = resetPasswordViewModel.NewPassword
                };

                status = await _accountService.ActionAccount(account, "ResetPassword");

                if (status)
                {
                    return Ok(new
                    {
                        Status = status,
                        ErrorMessage = errorMessage,
                    });
                }
            }
            catch (Exception)
            {
                errorMessage = "Something went wrong!";
            }

            return BadRequest(new
            {
                Status = status,
                ErrorMessage = errorMessage
            });
        }
        #endregion

        #region Refresh Tokens
        /// <summary>
        /// UC0-006 Refresh Tokens
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Refresh(string refreshToken)
        {
            try
            {
                var result = await _refreshTokenService.FindToken(refreshToken);

                if (result == null)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Refresh token does not exist!"
                    });
                }

                if (result.IsUsed)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Refresh token has been used!"
                    });
                }

                if (result.IsRevoked)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Refresh token has been revoked!"
                    });
                }

                if (result.ExpiredAt < DateTime.Now)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Refresh token has expired!"
                    });
                }

                var status = await _refreshTokenService.ChangeStatusRefreshToken(result);

                Account account = new()
                {
                    IdAccount = result.IdAccount
                };

                var user = await _accountService.GetAccount(account, "GetByID");

                var token = await GenerateToken(user);

                return Ok(new
                {
                    Sucess = true,
                    Message = "Refresh token successfully!",
                    Data = token
                });
            }
            catch (Exception)
            {
                return BadRequest(new
                {
                    error = "Something went wrong!",
                });
            }
        }
        #endregion

        #region Logout
        /// <summary>
        /// UC0-007
        /// Log out system
        /// </summary>   
        /// <returns>Specific HTTP Status code</returns>
        /// <response code="204">Return login page</response>
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Logout()
        {
            string rawUserId = HttpContext.User.FindFirstValue("UserID");

            if (String.IsNullOrEmpty(rawUserId))
            {
                return Unauthorized();
            }

            await _refreshTokenService.DeleteAllRefreshToken(rawUserId);

            return NoContent();
        }
        #endregion
    }
}
