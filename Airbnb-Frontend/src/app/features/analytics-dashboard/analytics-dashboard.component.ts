import { Component,OnInit } from '@angular/core';
import { NgChartsModule } from 'ng2-charts';
import { AdminStatisticsService } from '../../core/services/admin-statistics.service';
@Component({
  selector: 'app-analytics-dashboard',
  standalone: true,
  imports: [NgChartsModule],
  templateUrl: './analytics-dashboard.component.html',
  styleUrl: './analytics-dashboard.component.css'
})
export class AnalyticsDashboardComponent implements OnInit {
  BookingChartData :  any ;
  BookingChartOptions = {
    responsive: true,
    scales: {
      y: {
        beginAtZero: true
      }
    }
  };
  BookingChartType = 'bar'; // Change this to 'line', 'pie', etc. for different chart types
  BookingChartLegend = true; // Show legend
  BookingChartColors = [
    {
      backgroundColor: 'rgba(0, 123, 255, 0.2)',  // Blue for new bookings
      borderColor: 'rgba(0, 123, 255, 1)',
      borderWidth: 2
    },
    {
      backgroundColor: 'rgba(220, 53, 69, 0.2)',  // Red for cancellations
      borderColor: 'rgba(220, 53, 69, 1)',
      borderWidth: 2
    }
  ];
  // For Revenue Chart
  revenueChartData: any;
  revenueChartOptions = {
    responsive: true,
    scales: {
      y: {
        beginAtZero: true
      }
    }
  };
  revenueChartType = 'line'; // Change this to 'bar', 'pie', etc.
  revenueChartLegend = true;
  revenueChartColors = [
    {
      backgroundColor: 'rgba(75, 192, 192, 0.2)',
      borderColor: 'rgba(75, 192, 192, 1)',
      borderWidth: 1
    }
  ];
  userRoleChartData: any;
  userRoleChartType = 'pie';
  userRoleChartOptions = {
      responsive: true,
      plugins: {
        legend: {
          position: 'top' as const
        }
      }
    };
  userRoleChartLegend = true;
  topHostsChartData: any;
  topHostsChartOptions = {
    responsive: true,
    plugins: {
      legend: {
        position: 'top' as const
      }
    }
  };
  topHostsChartType = 'doughnut';
  topHostsChartLegend = true;
  topHostsChartColors = [
    {
      backgroundColor: [
        '#FF6384',
        '#36A2EB',
        '#FFCE56',
        '#4BC0C0',
        '#9966FF'
      ]
    }
  ];


  constructor(private statsService: AdminStatisticsService) {}
  ngOnInit(): void {
    // Fetch data from the API
    // this.statsService.getBookings().subscribe(data => {
    //   // Update chartData with the fetched data
    //   this.BookingChartData = {
    //     labels: data.labels, // Assuming the API returns labels
    //     datasets: [
    //       {
    //         label: 'Bookings',
    //         data: data.bookings, // Assuming the API returns bookings data
    //         backgroundColor: 'rgba(255, 99, 132, 0.2)',
    //         borderColor: 'rgba(255, 99, 132, 1)',
    //         borderWidth: 1
    //       }
    //     ]
    //   };
    // });
    this.statsService.getNewBookingsVsCancellations().subscribe(data => {
      this.BookingChartData = {
        labels: data.labels,  // Labels (Months)
        datasets: [
          {
            label: 'New Bookings',
            data: data.newBookingsData,
            // New bookings data
            borderColor: 'rgba(0, 123, 255, 1)',
            tension: 0.4
          },
          {
            label: 'Cancellations',
            data: data.cancellationsData,  // Cancellations data
            borderColor: 'rgba(220, 53, 69, 1)',
            tension: 0.4
          }
        ]
      };
    });
    this.statsService.getRevenue().subscribe(data => {
      // Update chartData with the fetched data
      this.revenueChartData = {
        labels: data.months, // Assuming the API returns labels
        datasets: [
          {
            label: 'Revenue Trend',
            data: data.revenue, // Assuming the API returns revenue data
            fill: true,
            backgroundColor: 'rgba(75, 192, 192, 0.2)',
            borderColor: 'rgba(75, 192, 192, 1)',
            borderWidth: 1,
            tension: 0.4
          }
        ]
      };
    });
    this.statsService.getUserDistribution().subscribe(data => {
      this.userRoleChartData = {
        labels: data.roles,
        datasets: [
          {
            label: 'User Roles',
            data: data.counts,
            backgroundColor: [
              'rgba(255, 99, 132, 0.2)',
              'rgba(54, 162, 235, 0.2)',
              'rgba(255, 206, 86, 0.2)',
              'rgba(75, 192, 192, 0.2)'
            ],
          }
        ]
      };
    });
    this.statsService.getTopHosts().subscribe(data => {
      this.topHostsChartData = {
        labels: data.hosts,
        datasets: [
          {
            label: 'Top Hosts by Bookings',
            data: data.bookingCounts,
            backgroundColor: [
              '#FF6384', '#36A2EB', '#FFCE56', '#4BC0C0', '#9966FF'
            ],
          }
        ]
      };
    });
    // this.statsService.getTopGuests().subscribe(data => {
    //   this.topGuestsChartData = {
    //     labels: data.labels,
    //     datasets: [
    //       {
    //         label: 'Top Guests by Bookings',
    //         data: data.data,
    //         backgroundColor: [
    //           '#FF6384', '#36A2EB', '#FFCE56', '#4BC0C0', '#9966FF'
    //         ],
    //       }
    //     ]
    //   };
    // })
  }
}


