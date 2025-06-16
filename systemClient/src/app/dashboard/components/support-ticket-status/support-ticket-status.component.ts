import { Component, inject, input } from '@angular/core';
import { AnalyticsService } from '../../../shared/services/analytics.service';
import { Chart, ChartTypeRegistry, registerables } from 'chart.js';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { Observable } from 'rxjs';
import { AsyncPipe } from '@angular/common';
import { UsersService } from '../../../shared/services/users.service';
import { Employee } from '../../../shared/types/users-types';

@Component({
  selector: 'app-support-ticket-status',
  imports: [
    FormsModule,
    MatInputModule,
    MatSelectModule,
    MatFormFieldModule,
    AsyncPipe,
  ],
  templateUrl: './support-ticket-status.component.html',
  styleUrl: './support-ticket-status.component.css',
})
export class SupportTicketStatusComponent {
  private readonly _analyticsService = inject(AnalyticsService);
  private readonly _usersService = inject(UsersService);

  chartType = input<string>('pie');
  selectedEmployeeId: string | null = null;

  employees$!: Observable<Employee[]>;

  ngOnInit(): void {
    Chart.register(...registerables);
    this.employees$ = this._usersService.getSupportUsersAnalytics();

    this.employees$.subscribe((employees) => {
      if (employees.length > 0) {
        this.selectedEmployeeId = employees[0].id;
        this.submit(this.selectedEmployeeId);
      }
    });
  }

  ngOnChanges() {
    if (this.employees$?.subscribe) {
      this.employees$.subscribe((employees) => {
        if (this.selectedEmployeeId) {
          this.submit(this.selectedEmployeeId);
        }
      });
    }
  }

  submit(id: string) {
    this.selectedEmployeeId = id;

    this._analyticsService.getSupportTicketStats(id).subscribe({
      next: (result) => {
        this._analyticsService.createPieChart(
          result,
          'supportTicketPieChart',
          this.chartType() as keyof ChartTypeRegistry
        );
      },
      error: (err) => {
        this._analyticsService.createPieChart(
          [],
          'supportTicketPieChart',
          this.chartType() as keyof ChartTypeRegistry
        );
      },
    });
  }
}
