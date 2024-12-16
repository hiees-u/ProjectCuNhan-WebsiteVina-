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
        beginAtZero: true,  // Đảm bảo trục Y bắt đầu từ 0
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

  constructor(private moderatorService: ModeratorService) {}

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
          // Chuyển startDate và endDate thành đối tượng Date nếu là chuỗi
          const start = new Date(this.startDate);
          const end = new Date(this.endDate);
          data = await this.moderatorService.getWeeklySalesReport(start, end);
          console.log('Dữ liệu nhận được:', data);
          this.prepareLineChart(data);
          break;
  
        case 'monthly':
          if (!this.selectedMonth || !this.selectedYear) {
            alert('Hãy chọn tháng và năm!');
            return;
          }
          data = await this.moderatorService.getMonthlySalesReport(this.selectedMonth, this.selectedYear);
          console.log('Dữ liệu nhận được:', data);
          this.prepareLineChart(data);
          break;
  
        case 'yearly':
          if (!this.selectedYear) {
            alert('Hãy nhập năm!');
            return;
          }
          data = await this.moderatorService.getYearlySalesReport(this.selectedYear);
          console.log('Dữ liệu nhận được:', data);
          this.prepareLineChart(data);
          break;
      }
    } catch (error) {
      console.error('Error generating report:', error);
      alert('Có lỗi xảy ra, vui lòng thử lại!');
    }
  }
  
  
  prepareBarChart(data: any) {
    if (!data?.data?.length) {
      alert('Không có dữ liệu để hiển thị biểu đồ!');
      return;
    }
    this.chartType = 'bar';
    this.chartData = {
      labels: data.data.map((d: any) => d.productName),
      datasets: [
        {
          label: 'Số lượng bán',
          data: data.data.map((d: any) => d.totalSales),
          backgroundColor: 'rgba(75, 192, 192, 0.2)',
          borderColor: 'rgba(75, 192, 192, 1)'
        },
      ],
    };
  }
  
  prepareLineChart(data: any) {
    if (!data?.length) {
      alert('Không có dữ liệu để hiển thị biểu đồ!');
      return;
    }
  
    // Tạo danh sách các sản phẩm và tổng số bán cho biểu đồ
    const productNames = data.map((d: any) => d.productName);
    const totalSales = data.map((d: any) => d.totalSales);
  
    this.chartType = 'line'; // Biểu đồ đường
    this.chartData = {
      labels: productNames,  // Tên sản phẩm sẽ là trục x
      datasets: [{
        label: 'Số lượng bán',
        data: totalSales, // Dữ liệu số lượng bán sẽ là trục y
        borderColor: 'rgba(54, 162, 235, 1)',  // Màu đường biểu đồ
        backgroundColor: 'rgba(54, 162, 235, 0.2)', // Màu nền  // Không điền màu dưới đường
      }]
    };
  }
  
  
}
