import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
    selector: 'app-search-box',
    imports: [
        FormsModule,
    ],
    templateUrl: './search-box.component.html',
    styleUrl: './search-box.component.css'
})
export class SearchBoxComponent {
  searchBox: string = '';
  @Output() products = new EventEmitter<string>();

  onSearch() {
    console.log(`Gửi đi ${this.searchBox}`);
    this.products.emit(this.searchBox)
  }

  resetSearchBox() {
    this.searchBox = '';
    this.onSearch();
  }
}
