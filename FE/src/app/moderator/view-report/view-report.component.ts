import { Component } from '@angular/core';
import { ModeratorService } from '../moderator.service';
import { FormsModule } from '@angular/forms';  // Đảm bảo FormsModule được import vào component này
import { CommonModule } from '@angular/common';
import { ChartData, ChartOptions, ChartType } from 'chart.js';  // Đảm bảo bạn đã cài chart.js
import { BaseChartDirective } from 'ng2-charts';
import { Chart, registerables } from 'chart.js';

Chart.register(...registerables);

@Component({
  selector: 'app-view-report',
  standalone: true,  // Nếu sử dụng Standalone Component
  imports: [CommonModule, FormsModule, BaseChartDirective],  // Import FormsModule ở đây
  templateUrl: './view-report.component.html',
  styleUrls: ['./view-report.component.css'],
})
export class ViewReportComponent {
  reportType: string = 'daily';
  selectedDate: Date | null = null;
  startDate: Date | null = null;
  endDate: Date | null = null;
  selectedMonth: number | null = null;
  selectedYear: number | null = null;

  chartData = {
    labels: ['January', 'February', 'March'],
    datasets: [
      {
        data: [65, 59, 80],
        label: 'Doanh thu',
        backgroundColor: 'rgba(75,192,192,0.4)',
        borderColor: 'rgba(75,192,192,1)',
      },
    ],
  };
  chartType: ChartType = 'bar';
  chartOptions: ChartOptions = {
    responsive: true,
    maintainAspectRatio: false,
    scales: {
        y: {
            beginAtZero: true,
            ticks: {
                stepSize: 200000,
                callback: function (value) {
                    return value.toLocaleString();
                },
            },
        },
        x: {
            ticks: {
                autoSkip: false,
                maxRotation: 45,
                minRotation: 0,
            },
        },
    },
    plugins: {
        legend: {
            display: true,
            position: 'top',
        },
    },
  };



  months = Array.from({ length: 12 }, (_, i) => i + 1);

  tableData: any[] = [];

  constructor(private moderatorService: ModeratorService) { }

  async generateReport() {
    try {
      let data: any;
      switch (this.reportType) {
        case 'daily':
          if (!this.selectedDate) {
            alert('Hãy chọn ngày!');
            return;
          }
          const date = new Date(this.selectedDate); // Chuyển chuỗi thành đối tượng Date
          data = await this.moderatorService.getDailySalesReport(date);
          this.prepareBarChart(data);
          break;
          case 'weekly':
        if (!this.startDate || !this.endDate) {
          alert('Hãy chọn khoảng thời gian!');
          return;
        }
        const startdate = new Date(this.startDate);
        const enddate = new Date(this.endDate);
        data = await this.moderatorService.getWeeklySalesReport(startdate,enddate);
        this.prepareLineChart(data);
        break;

      case 'monthly':
        if (!this.selectedMonth || !this.selectedYear) {
          alert('Hãy chọn tháng và năm!');
          return;
        }
        const month = this.selectedMonth;
        const year = this.selectedYear;
        data = await this.moderatorService.getMonthlySalesReport(month, year);
        console.log('Dữ liệu thống kê theo tháng:', data); // Kiểm tra dữ liệu trả về
        this.prepareLineChartMonth(data);
        break;

      case 'yearly':
        if (!this.selectedYear) {
          alert('Hãy nhập năm!');
          return;
        }
        data = await this.moderatorService.getYearlySalesReport(this.selectedYear);
        console.log('Dữ liệu thống kê theo năm:', data); 
        this.prepareLineChartYear(data);
        break;
    }
    if (data?.data?.length) {
      this.tableData = data.data; // Gán dữ liệu vào bảng
      this.updateChart(data.data); // Cập nhật biểu đồ
    } else {
      alert('Không có dữ liệu để hiển thị!');
      this.tableData = [];
      this.chartData = { labels: [], datasets: [] };
    }
  } catch (error) {
    console.error('Error generating report:', error);
    alert('Có lỗi xảy ra, vui lòng thử lại!');
  }
}

updateChart(data: any[]) {
  if (!data?.length) {
    this.chartData = { labels: [], datasets: [] };
    this.tableData = [];
    return;
  }

  if (this.reportType === 'yearly') {
    // Dữ liệu tổng quan theo năm
    this.chartType = 'line';
    this.chartData = {
      labels: data.map((item: any) => `Tháng ${item.month}`),
      datasets: [
        {
          label: 'Doanh thu (VND)',
          data: data.map((item: any) => item.totalRevenue),
          borderColor: 'rgba(54, 162, 235, 1)',
          backgroundColor: 'rgba(54, 162, 235, 0.2)',
        },
      ],
    };
    this.tableData = data.map((item: any) => ({
      month: item.month,
      totalSales: item.totalSales,
      totalRevenue: item.totalRevenue,
    }));
  } else {
    // Dữ liệu chi tiết cho ngày, tuần, tháng
    this.chartType = this.reportType === 'daily' ? 'bar' : 'line';
    this.chartData = {
      labels: data.map((item: any) =>
        this.reportType === 'monthly' ? `Ngày ${item.day}` : item.productName
      ),
      datasets: [
        {
          label: 'Doanh thu (VND)',
          data: data.map((item: any) => item.totalRevenue),
          borderColor: 'rgba(54, 162, 235, 1)',
          backgroundColor: 'rgba(54, 162, 235, 0.2)',
        },
      ],
    };
    this.tableData = data.map((item: any) => ({
      productName: item.productName,
      totalSales: item.totalSales,
      totalRevenue: item.totalRevenue,
    }));
  }
}
  prepareBarChart(data: any) {
    if (!data?.data?.length) {
      alert('Không có dữ liệu để hiển thị biểu đồ!');
      return;
    }
    this.chartType = 'bar';
    this.chartData = {
      labels: data.data.map((item: any) => item.productName),
      datasets: [
        {
          label: 'Số lượng bán',
          data: data.data.map((item: any) => item.totalSales),
          backgroundColor: 'rgba(75, 192, 192, 0.2)',
          borderColor: 'rgba(75, 192, 192, 1)',
        },
      ],
    };

  }

  prepareLineChart(data: any) {
    if (!data?.data?.length) {
      alert('Không có dữ liệu để hiển thị biểu đồ!');
      return;
    }
    this.chartType = 'line';
    this.chartData = {
      labels: data.data.map((item: any) => item.startDate), // Hoặc xử lý để format ngày
      datasets: [
        {
          label: 'Doanh thu',
          data: data.data.map((item: any) => item.totalRevenue),
          borderColor: 'rgba(54, 162, 235, 1)',
          backgroundColor: 'rgba(54, 162, 235, 0.2)',
        },
      ],
    };
  }

  prepareLineChartMonth(data: any) {
    if (!data?.data?.length) {
      alert('Không có dữ liệu để hiển thị biểu đồ!');
      return;
    }
  
    // Xử lý dữ liệu cho biểu đồ
    this.chartType = 'line';
    this.chartData = {
      labels: data.data.map((item: any) => `Ngày ${item.day}`), // Dùng `Day` làm trục X
      datasets: [
        {
          label: 'Doanh thu (VND)',
          data: data.data.map((item: any) => item.totalRevenue), // Doanh thu làm dữ liệu
          borderColor: 'rgba(54, 162, 235, 1)',
          backgroundColor: 'rgba(54, 162, 235, 0.2)',
        },
      ],
    };
  }
  
  prepareLineChartYear(data: any) {
    if (!data?.data?.length) {
      alert('Không có dữ liệu để hiển thị biểu đồ!');
      return;
    }
  
    // Xử lý dữ liệu cho biểu đồ
    const isYearly = this.reportType === 'yearly';
    this.chartType = 'line';
    this.chartData = {
      labels: isYearly
        ? data.data.map((item: any) => `Tháng ${item.month}`) // Dùng `Month` làm trục X
        : data.data.map((item: any) => `Ngày ${item.day}`), // Dùng `Day` cho thống kê tháng
      datasets: [
        {
          label: isYearly ? 'Doanh thu (VND) theo tháng' : 'Doanh thu (VND) theo ngày',
          data: data.data.map((item: any) => item.totalRevenue),
          borderColor: 'rgba(54, 162, 235, 1)',
          backgroundColor: 'rgba(54, 162, 235, 0.2)',
        },
      ],
    };
  }
  
}
