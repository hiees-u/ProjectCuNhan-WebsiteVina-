import { Component, EventEmitter, Output } from '@angular/core';
import { WarehouseEmployeeService } from '../warehouse-employee.service';
import {
    Warehouse, purchaseOrderIdsResponse,
    PurchaseOrderDetailResponse,
    ContructorPurchaseOrderDetailResponseModule
}
    from '../warehouse-employee.module';

import { BaseResponseModel, BaseResponseModule }
    from '../../shared/module/base-response/base-response.module';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { CommonModule } from '@angular/common';

import { ImportWarehouseRequest, ContructorRequestImportWarehouseModule }
    from '../warehouse-employee.module';
@Component({
    selector: 'app-in-warehouse',
    imports: [FormsModule, ReactiveFormsModule,
        CommonModule],
    templateUrl: './in-warehouse.component.html',
    styleUrl: './in-warehouse.component.css'
})
export class InWarehouseComponent {
    @Output() isClose: EventEmitter<boolean> = new EventEmitter<boolean>();
    constructor(private warehouseEmployeeService: WarehouseEmployeeService) { }
    ngOnInit(): void {
        this.getWarehouses();
        this.GetUndeliveredPurchaseOrders();
    }
    importWarehouse: ImportWarehouseRequest = ContructorRequestImportWarehouseModule();
    isAdd: boolean = false;

    warehouses: Warehouse[] = [];
    purchaseOrderIds: purchaseOrderIdsResponse[] = [];
    products: PurchaseOrderDetailResponse[] = [];
    selectedpurchaseOrderId: number = 0;

    async getWarehouses() {
        const data = await this.warehouseEmployeeService.getWarehouses();
        this.warehouses = data.data;
    }
    async GetUndeliveredPurchaseOrders() {
        const data = await this.warehouseEmployeeService.GetUndeliveredPurchaseOrders();
        this.purchaseOrderIds = data.data;
        console.error('purchaseOrderId lấy được: ', data)
    }

    async onPurchaseOrderChange(event: Event) {
        const target = event.target as HTMLSelectElement;
        if (target) {
            const PurchaseOrderId = Number(target.value);
            this.selectedpurchaseOrderId = PurchaseOrderId;

            if (PurchaseOrderId > 0) {
                try {
                    const data = await this.warehouseEmployeeService.GetPurchaseOrderDetails(PurchaseOrderId);
                    if (data && data.data && Array.isArray(data.data)) {
                        // Áp dụng hàm map với cấu trúc mặc định
                        this.products = data.data.map((product: Partial<PurchaseOrderDetailResponse>) => {
                            const detail = ContructorPurchaseOrderDetailResponseModule();
                            return { ...detail, ...product }; // Kết hợp cấu trúc mặc định và dữ liệu API
                        });
                    } else {
                        this.products = [];
                    }
                } catch (error) {
                    console.error('Error fetching order details:', error);
                    this.products = [];
                }
            } else {
                this.products = [];
            }
        }
    }

    updateQuantityToImport(event: Event, product: PurchaseOrderDetailResponse): void {
        const inputElement = event.target as HTMLInputElement;
        const inputValue = Number(inputElement.value);

        // Kiểm tra và gán giá trị hợp lệ
        if (isNaN(inputValue) || inputValue < 0) {
            alert('Số lượng nhập không thể nhỏ hơn 0.');
            product.quantityToImport = 0; // Reset về 0 nếu giá trị không hợp lệ
        } else if (inputValue > product.quantityOrdered - product.quantityDelivered) {
            alert('Số lượng nhập không được vượt quá số lượng còn lại để giao!');
            product.quantityToImport = product.quantityOrdered - product.quantityDelivered; // Reset về max
        } else {
            product.quantityToImport = inputValue; // Gán giá trị hợp lệ
        }

        console.error('Giá trị quantityToImport cập nhật:', product.quantityToImport);
    }


    async submitForm() {
        if (!this.importWarehouse.warehouseID) {
            alert('Vui lòng chọn kho hàng!');
            return;
        }

        if (!this.products || this.products.length === 0) {
            alert('Danh sách sản phẩm trống. Vui lòng kiểm tra lại!');
            return;
        }

        // Kiểm tra các sản phẩm không hợp lệ
        const invalidProducts = this.products.filter(product => {
            const maxQuantity = product.quantityOrdered - product.quantityDelivered;
            return (
                product.quantityToImport <= 0 || // Số lượng nhập <= 0
                !product.cellId ||              // Chưa chọn ô lưu trữ
                product.quantityToImport > maxQuantity // Vượt quá số lượng cho phép
            );
        });

        if (invalidProducts.length > 0) {
            alert('Có sản phẩm chưa chọn ô lưu trữ hoặc số lượng nhập không hợp lệ (Vượt quá số lượng cho phép). Vui lòng kiểm tra lại!');
            return;
        }

        const receiptDetails = this.products.map(product => ({
            productID: product.productId,
            cellID: product.cellId,
            quantity: product.quantityToImport,
            purchaseOrderID: this.selectedpurchaseOrderId,
        }));

        const importData: ImportWarehouseRequest = {
            warehouseID: this.importWarehouse.warehouseID,
            receiptDetails: receiptDetails,
        };

        try {
            const response = await this.warehouseEmployeeService.importWarehouseByPurchaseOrder(importData);
            if (response && response.isSuccess) {
                alert('Lưu phiếu nhập kho thành công!');
                this.isClose.emit(true);
            } else {
                alert(response.message || 'Có lỗi xảy ra. Vui lòng thử lại!');
            }
        } catch (error) {
            console.error('Error submitting import request:', error);
            alert('Không thể lưu phiếu nhập kho. Vui lòng thử lại sau!');
        }
    }

}