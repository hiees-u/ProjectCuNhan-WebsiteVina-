import { AfterViewInit, Component, ElementRef, ViewChild } from '@angular/core';

@Component({
  selector: 'app-filter-price',
  standalone: true,
  imports: [],
  templateUrl: './filter-price.component.html',
  styleUrl: './filter-price.component.css',
})
export class FilterPriceComponent implements AfterViewInit {
  @ViewChild('lowerSlider', { static: false }) lowerSlider!: ElementRef;
  @ViewChild('upperSlider', { static: false }) upperSlider!: ElementRef;
  @ViewChild('one', { static: false }) oneInput!: ElementRef;
  @ViewChild('two', { static: false }) twoInput!: ElementRef;

  lowerValue: number = 100; // giá trị mặc định
  upperValue: number = 500; // giá trị mặc định

  ngAfterViewInit(): void {
    this.updateValues();

    // Thêm sự kiện cho slider trên
    this.upperSlider.nativeElement.oninput = () => {
      this.handleUpperInput();
    };

    // Thêm sự kiện cho slider dưới
    this.lowerSlider.nativeElement.oninput = () => {
      this.handleLowerInput();
    };
  }

  updateValues() {
    // Cập nhật giá trị từ slider vào input
    this.lowerValue = parseInt(this.lowerSlider.nativeElement.value);
    this.upperValue = parseInt(this.upperSlider.nativeElement.value);
    this.oneInput.nativeElement.value = this.lowerValue;
    this.twoInput.nativeElement.value = this.upperValue;
  }

  handleUpperInput() {
    this.updateValues();
    if (this.upperValue < this.lowerValue + 4) {
      this.lowerSlider.nativeElement.value = this.upperValue - 4;
      if (this.lowerValue === parseInt(this.lowerSlider.nativeElement.min)) {
        this.upperSlider.nativeElement.value = 4;
      }
    }
    this.upperValue = parseInt(this.upperSlider.nativeElement.value);
    this.twoInput.nativeElement.value = this.upperValue;
  }

  handleLowerInput() {
    this.updateValues();
    if (this.lowerValue > this.upperValue - 4) {
      this.upperSlider.nativeElement.value = this.lowerValue + 4;
      if (this.upperValue === parseInt(this.upperSlider.nativeElement.max)) {
        this.lowerSlider.nativeElement.value = parseInt(this.upperSlider.nativeElement.max) - 4;
      }
    }
    this.lowerValue = parseInt(this.lowerSlider.nativeElement.value);
    this.oneInput.nativeElement.value = this.lowerValue;
  }
}
