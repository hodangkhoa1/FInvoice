const chartCustomer = document.getElementById("ChartCustomer").getContext("2d");
const MONTHS = [
    "January",
    "February",
    "March",
    "April",
    "May",
    "June",
    "July",
    "August",
    "September",
    "October",
    "November",
    "December",
];

var myChart = new Chart(chartCustomer, {
    type: "line",
    data: {
        labels: MONTHS,
        datasets: [
            {
                label: "Customers",
                data: [12, 19, 25, 5, 2, 3, 21, 50, 5, 10],
                fill: false,
                borderColor: "rgb(75, 192, 192)",
                tension: 0.1,
            },
        ],
    },
    options: {
        response: true,
    },
});

$(".counter").counterUp({
    delay: 10,
    time: 1200,
});