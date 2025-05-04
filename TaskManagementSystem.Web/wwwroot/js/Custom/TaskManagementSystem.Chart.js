TaskManagementSystem.Chart = new function () {

    this.loadChartData = function () {
        const startDate = $('#startDate').val();
        const endDate = $('#endDate').val();
        const userId = $('#userDropdown').val();

        $.ajax({
            url: '/Dashboard/GetChartData',
            type: 'GET',
            data: {
                startDate: startDate,
                endDate: endDate,
                userId: userId
            },
            success: function (data) {
              
                TaskManagementSystem.Chart.updatePieChart(data.statusLabels, data.statusData);
                TaskManagementSystem.Chart.updateBarChart(data.priorityLabels, data.priorityData);
            },
            error: function (xhr, status, error) {
                console.error('Error fetching chart data:', error);
            }
        });
    }
    this.updatePieChart = function (labels, data) {
        const ctx = document.getElementById("pie-chart");

        if (pieChartInstance) pieChartInstance.destroy();

        // Generate random colors for each slice
        const randomColors = labels.map(() => {
            return `hsl(${Math.floor(Math.random() * 360)}, 70%, 60%)`;
        });

        pieChartInstance = new Chart(ctx, {
            type: 'pie',
            data: {
                labels: labels,
                datasets: [{
                    data: data,
                    backgroundColor: randomColors
                }]
            },
            options: {
                responsive: true
            }
        });
    }


    this.updateBarChart = function (labels, data) {
        const ctx = document.getElementById("bar-chart");

        // Destroy existing chart instance
        if (barChartInstance) barChartInstance.destroy();

        // Generate random colors for each bar
        const randomColors = labels.map(() => {
            return `hsl(${Math.floor(Math.random() * 360)}, 70%, 60%)`; // HSL for vibrant colors
        });

        barChartInstance = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: labels,
                datasets: [{
                    label: 'Task Count',
                    data: data,
                    backgroundColor: randomColors
                }]
            },
            options: {
                responsive: true,
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            }
        });
    }

}