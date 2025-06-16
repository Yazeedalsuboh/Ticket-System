import { Component, inject, input, OnInit } from '@angular/core';
import { AnalyticsService } from '../../../shared/services/analytics.service';
import { Chart, ChartTypeRegistry, registerables } from 'chart.js';

@Component({
  selector: 'app-ticket-status',
  imports: [],
  templateUrl: './ticket-status.component.html',
  styleUrl: './ticket-status.component.css',
})
export class TicketStatusComponent implements OnInit {
  private readonly _analyticsService = inject(AnalyticsService);
  chartType = input<string>('pie');

  ngOnInit(): void {
    Chart.register(...registerables);
  }

  ngOnChanges(): void {
    this._analyticsService.getTicketStatus().subscribe({
      next: (result) => {
        this._analyticsService.createPieChart(
          result,
          'ticketPieChart',
          this.chartType() as keyof ChartTypeRegistry
        );
      },
      error: (err) => {
        this._analyticsService.createPieChart(
          [],
          'ticketPieChart',
          this.chartType() as keyof ChartTypeRegistry
        );
      },
    });
  }
}
