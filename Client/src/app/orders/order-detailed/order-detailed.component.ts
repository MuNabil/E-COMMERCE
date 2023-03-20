import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IOrder } from 'src/app/shared/models/order';
import { BreadcrumbService } from 'xng-breadcrumb';
import { OrdersService } from '../orders.service';

@Component({
  selector: 'app-order-detailed',
  templateUrl: './order-detailed.component.html',
  styleUrls: ['./order-detailed.component.scss']
})
export class OrderDetailedComponent implements OnInit {
  order?: IOrder;

  constructor(private ordersService: OrdersService, private route: ActivatedRoute,
    private bcService: BreadcrumbService) {

    this.bcService.set('@orderDetailed', ' ');
  }

  ngOnInit(): void {
    const orderId = +this.route.snapshot.paramMap.get('id');
    this.getOrderDetailed(orderId);
  }

  getOrderDetailed(orderId: number) {
    this.ordersService.gerOrderDetailed(orderId).subscribe(order => {
      this.order = order;
      this.bcService.set('@orderDetailed', `Order# ${order.id} - ${order.status}`);
    });
  }

}
