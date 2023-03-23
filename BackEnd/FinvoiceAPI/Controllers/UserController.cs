using BAL.Enums;
using BAL.Models;
using BAL.Services.Interfaces;
using BAL.Validators;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace FinvoiceAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IRoleService _roleService;

        public UserController(IAccountService accountService, IRoleService roleService)
        {
            _accountService = accountService;
            _roleService = roleService;
        }

        #region Get Profile User
        /// <summary>
        /// UC0-001
        /// Get Profile User
        /// </summary>
        /// <returns>Specific HTTP Status code</returns>
        /// <response code="200">Return profile screen if the access is successful</response>
        /// <response code="404">If the profile is not found</response>
        [HttpGet]
        [ProducesResponseType(typeof(UserInfoViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GetProfileUser(string userID)
        {
            string errorMessage = "";
            bool status = false;

            try
            {
                if (string.IsNullOrEmpty(userID))
                {
                    return BadRequest(new
                    {
                        Status = status,
                        ErrorMessage = "Something went wrong!"
                    });
                }

                Account account = new()
                {
                    IdAccount = userID
                };

                Account getAccount = await _accountService.GetAccount(account, "GetByID");
                RoleViewModel getRole = await _roleService.GetRoleTask(new Role()
                {
                    IdRole = getAccount.UserRole
                }, "GetRoleID");

                UserInfoViewModel userInfoViewModel = new()
                {
                    IdAccount = getAccount.IdAccount,
                    FullName = getAccount.FullName,
                    Email = getAccount.Email,
                    Password = getAccount.Password,
                    DateOfBirth = getAccount.DateOfBirth,
                    Address = getAccount.Address,
                    Phone = getAccount.Phone,
                    Gender = getAccount.Gender,
                    Avatar = getAccount.Avatar,
                    TaxCode = getAccount.TaxCode,
                    ColorAvatar = getAccount.ColorAvatar,
                    DefaultAvatar = getAccount.DefaultAvatar,
                    UserRole = getRole,
                    Status = getAccount.Status,
                    AccountCreated = getAccount.AccountCreated
                };

                if (userInfoViewModel != null)
                {
                    status = true;

                    return new JsonResult(new
                    {
                        Data = userInfoViewModel,
                        Status = status,
                        ErrorMessage = errorMessage
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

        #region Edit Profile
        /// <summary>
        /// UC0-002
        /// Edit Profile
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///
        ///         {
        ///           "FullName": "test",
        ///           "Gender": "M",
        ///           "DateOfBirth": "30/12/2022",
        ///           "Email": "test@gmail.com",
        ///           "Phone": "09xxxxxxxx",
        ///           "Address": "Quan 9",
        ///           "Avatar": "import picture"
        ///         }
        ///         
        /// </remarks>
        /// <returns>Specific HTTP Status code</returns>
        /// <response code="200">Return profile if edit is successful</response>
        /// <response code="400"></response>
        [HttpPost()]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> EditProfile([FromBody] EditProfileViewModel editProfileViewModel)
        {
            string errorMessage;
            bool status = false;
            EditProfileValidator editProfileValidator = new();

            try
            {
                if (editProfileViewModel == null)
                {
                    throw new Exception("Api's information has been corrupted, please try again or contact developer for more support");
                }

                var validation = editProfileValidator.Validate(editProfileViewModel);

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
                        ErrorMessage = errors
                    });
                }

                Account account = new()
                {
                    FullName = editProfileViewModel.FullName,
                    Email = editProfileViewModel.Email,
                    DateOfBirth = editProfileViewModel.DateOfBirth,
                    Address = editProfileViewModel.Address,
                    Phone = editProfileViewModel.Phone,
                    Gender = editProfileViewModel.Gender,
                    Avatar = editProfileViewModel.Avatar,
                };

                bool checkEditProfile = await _accountService.ActionAccount(account, "EditProfile");

                if (checkEditProfile == false)
                {
                    errorMessage = "Account does not exist!";

                    return BadRequest(new
                    {
                        Status = status,
                        ErrorMessage = errorMessage
                    });
                }

                errorMessage = "Edit profile successfully!";
                status = true;
            }
            catch (Exception)
            {
                errorMessage = "Something went wrong!";
            }

            return Ok(new
            {
                Status = status,
                ErrorMessage = errorMessage
            });
        }
        #endregion

        #region Change Password
        /// <summary>
        /// UC0-003
        /// Change Password
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///
        ///         {
        ///           "UserEmail": "test@gmail.com",
        ///           "NewPassword": "test",
        ///           "ComrfirmPassword": "test",
        ///         }
        ///         
        /// </remarks>
        /// <returns>Specific HTTP Status code</returns>
        /// <response code="200">Return profile if edit is successful</response>
        /// <response code="400"></response>
        [HttpPost()]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> ChangePassword([FromBody] ResetPasswordViewModel changePasswordViewModel)
        {
            string errorMessage;
            bool status = false;
            ResetPasswordValidator changePasswordValidator = new();

            try
            {
                if (changePasswordViewModel == null)
                {
                    throw new Exception("Api's information has been corrupted, please try again or contact developer for more support");
                }

                var validation = changePasswordValidator.Validate(changePasswordViewModel);

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
                        ErrorMessage = errors
                    });
                }

                Account account = new()
                {
                    Email = changePasswordViewModel.UserEmail,
                    Password = changePasswordViewModel.NewPassword
                };

                var getAccount = await _accountService.GetAccount(account, "GetByEmail");

                if (getAccount == null)
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
                    bool isValidPassword = BCrypt.Net.BCrypt.Verify(account.Password, getAccount.Password);

                    if (isValidPassword == true)
                    {
                        errorMessage = "The new password cannot be the same as the old password!";

                        return BadRequest(new
                        {
                            Status = status,
                            ErrorMessage = errorMessage
                        });
                    }
                    else
                    {
                        bool checkChangePassword = await _accountService.ActionAccount(account, "ChangePassword");

                        if (checkChangePassword == false)
                        {
                            errorMessage = "Cannot change password! Please try again!";

                            return BadRequest(new
                            {
                                Status = status,
                                ErrorMessage = errorMessage
                            });
                        }

                        errorMessage = "Change password successfully!";
                        status = true;
                    }
                }
            }
            catch (Exception)
            {
                errorMessage = "Something went wrong!";
            }

            return Ok(new
            {
                Status = status,
                ErrorMessage = errorMessage
            });
        }
        #endregion

        #region Change Status User In Admin
        /// <summary>
        /// UC0-001
        /// Change Status User In Admin
        /// </summary>
        /// <returns>Specific HTTP Status code</returns>
        /// <response code="200">Return list user screen if change status is successful</response>
        /// <response code="404"></response>
        [HttpPost()]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeStatusUser(string userID, string actionChange)
        {
            string errorMessage;
            bool status = false;

            try
            {
                if (string.IsNullOrEmpty(userID))
                {
                    return BadRequest(new
                    {
                        Status = status,
                        ErrorMessage = "Something went wrong!"
                    });
                }
                else if (string.IsNullOrEmpty(actionChange))
                {
                    return BadRequest(new
                    {
                        Status = status,
                        ErrorMessage = "Something went wrong!"
                    });
                }

                Account account = new()
                {
                    IdAccount = userID,
                };

                if (actionChange.Equals(UserStatusEnum.Enable))
                {
                    account.Status = 1;
                }
                else if (actionChange.Equals(UserStatusEnum.Disable))
                {
                    account.Status = 2;
                }

                bool checkEditStatus = await _accountService.ActionAccount(account, "EditStatus");

                if (checkEditStatus == false)
                {
                    errorMessage = "Account does not exist!";

                    return BadRequest(new
                    {
                        Status = status,
                        ErrorMessage = errorMessage
                    });
                }

                errorMessage = "Change status successfully!";
                status = true;
            }
            catch (Exception)
            {
                errorMessage = "Something went wrong!";
            }

            return Ok(new
            {
                Status = status,
                ErrorMessage = errorMessage
            });
        }
        #endregion

        #region Get all list
        /// <summary>
        /// UC-26
        /// Get all list user
        /// </summary>
        /// <returns>Specific HTTP Status code</returns>
        /// <response code="200">Return list screen if the access is successful</response>
        /// <response code="404">If the list is not found</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<Account>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllListUser(int page)
        {
            string errorMessage = "";
            bool status = false;

            try
            {
                Account account = new()
                {
                    UserRole = "L5uojNlToi"
                };

                int index = page;

                List<Account> result = await _accountService.GetAllAccount(account, "PagingAccount");
                int getTotal = result.Count;

                if (result.Count != 0)
                {
                    result = result.Skip((index - 1) * 5).Take(5).ToList();
                }

                int endPage = getTotal / 5;

                if (getTotal % 5 != 0)
                {
                    endPage++;
                }

                return Ok(new
                {
                    EndPage = endPage,
                    CurrentPage = index,
                    Status = true,
                    Data = result
                });
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
    }
}
