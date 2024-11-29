import { TestBed } from '@angular/core/testing';

import { WarehouseEmployeeService } from './warehouse-employee.service';

describe('WarehouseEmployeeService', () => {
  let service: WarehouseEmployeeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(WarehouseEmployeeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
