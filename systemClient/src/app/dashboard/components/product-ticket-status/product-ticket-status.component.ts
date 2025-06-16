import { Component, inject, input } from '@angular/core';
import { AnalyticsService } from '../../../shared/services/analytics.service';
import { Chart, ChartTypeRegistry, registerables } from 'chart.js';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { Observable } from 'rxjs';
import { TicketsService } from '../../../shared/services/tickets.service';
import { AsyncPipe } from '@angular/common';
import { Product } from '../../../shared/types/products-types';
import { TranslatePipe } from '@ngx-translate/core';

@Component({
  selector: 'app-product-ticket-status',
  imports: [
    FormsModule,
    MatInputModule,
    MatSelectModule,
    MatFormFieldModule,
    AsyncPipe,
    TranslatePipe,
  ],
  templateUrl: './product-ticket-status.component.html',
  styleUrl: './product-ticket-status.component.css',
})
export class ProductTicketStatusComponent {
  private readonly _analyticsService = inject(AnalyticsService);
  private readonly _ticketsService = inject(TicketsService);
  chartType = input<string>('pie');
  selectedProductId: string | null = null;

  products$!: Observable<Product[]>;

  ngOnInit(): void {
    Chart.register(...registerables);
    this.products$ = this._ticketsService.getProducts();
    this.products$.subscribe((products) => {
      if (products.length > 0) {
        this.selectedProductId = products[0].id;
        this.submit(this.selectedProductId);
      }
    });
  }

  ngOnChanges() {
    if (this.products$?.subscribe) {
      this.products$.subscribe((product) => {
        if (this.selectedProductId) {
          this.submit(this.selectedProductId);
        }
      });
    }
  }

  submit(id: string) {
    this.selectedProductId = id;
    this._analyticsService.getProductTicketStats(id).subscribe({
      next: (result) => {
        this._analyticsService.createPieChart(
          result,
          'productTicketPieChart',
          this.chartType() as keyof ChartTypeRegistry
        );
      },
      error: (err) => {
        this._analyticsService.createPieChart(
          [],
          'productTicketPieChart',
          this.chartType() as keyof ChartTypeRegistry
        );
      },
    });
  }
}
