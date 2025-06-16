import { Component, inject, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AnalyticsService } from '../../../shared/services/analytics.service';
import { AsyncPipe, NgIf } from '@angular/common';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-summary',
  imports: [AsyncPipe, MatCardModule],
  templateUrl: './summary.component.html',
  styleUrl: './summary.component.css',
})
export class SummaryComponent implements OnInit {
  private readonly _analyticsService = inject(AnalyticsService);

  data$!: Observable<
    {
      title: string;
      amount: number;
    }[]
  >;

  ngOnInit(): void {
    this.data$ = this._analyticsService.getSummary();
  }
}
