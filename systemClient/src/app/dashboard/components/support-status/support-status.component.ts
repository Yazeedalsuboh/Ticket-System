import { Component, inject, input, OnInit } from '@angular/core';
import { AnalyticsService } from '../../../shared/services/analytics.service';
import { Chart, ChartTypeRegistry, registerables } from 'chart.js';

@Component({
  selector: 'app-support-status',
  templateUrl: './support-status.component.html',
  styleUrl: './support-status.component.css',
})
export class SupportStatusComponent implements OnInit {
  private readonly _analyticsService = inject(AnalyticsService);
  chartType = input<string>('pie');

  ngOnInit(): void {
    Chart.register(...registerables);
  }

  ngOnChanges(): void {
    this._analyticsService.getSupportStatus().subscribe({
      next: (result) =>
        this._analyticsService.createPieChart(
          result,
          'supportPieChart',
          this.chartType() as keyof ChartTypeRegistry
        ),
      error: () =>
        this._analyticsService.createPieChart(
          [],
          'supportPieChart',
          this.chartType() as keyof ChartTypeRegistry
        ),
    });
  }
}
