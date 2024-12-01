import {
  Component,
  EventEmitter,
  Input,
  Output,
  SimpleChanges,
} from '@angular/core';
import { ServicesService } from '../../services.service';
import {
  Address,
  ConstructorAddress,
} from '../../module/address/address.module';
import { FormsModule } from '@angular/forms';

@Component({
    selector: 'app-address',
    imports: [FormsModule],
    templateUrl: './address.component.html',
    styleUrl: './address.component.css'
})
export class AddressComponent {
  @Input() address: Address = ConstructorAddress();
  @Output() addressChange = new EventEmitter<Address>();

  constructor(private service: ServicesService) {}

  ngOnInit(): void {
  }

  onInputChange() {
    this.addressChange.emit(this.address);
  }
}
