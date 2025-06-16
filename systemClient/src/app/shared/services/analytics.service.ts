import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { catchError, map, Observable, throwError } from 'rxjs';
import { Chart, ChartTypeRegistry } from 'chart.js';
import { GeneralService } from './general.service';
import { Stat } from '../types/analytics-types';
import { responseData } from '../types/responses-types';

@Injectable({
  providedIn: 'root',
})
export class AnalyticsService {
  private readonly _apiUrl: string = environment.apiUrl;
  private readonly _httpClient = inject(HttpClient);
  private readonly _generalService = inject(GeneralService);

  public getSupportStatus(): Observable<Stat[]> {
    return this._httpClient
      .get<responseData>(`${this._apiUrl}/api/dashboard/support-stats`)
      .pipe(
        map((res) => {
          const transformedData = [
            { label: 'Active', value: res.data.activeSupport },
            { label: 'InActive', value: res.data.inactiveSupport },
          ];
          return transformedData;
        }),
        catchError((error) => {
          const customError = new Error(
            'Failed to get support status: ' + error.message
          );
          this._generalService.openSnackBar(customError.message);
          return throwError(() => {});
        })
      );
  }

  public getTicketStatus(): Observable<Stat[]> {
    return this._httpClient
      .get<responseData>(`${this._apiUrl}/api/dashboard/ticket-stats`)
      .pipe(
        map((res) => {
          const transformedData = [
            { label: 'Assigned', value: res.data.assigned },
            { label: 'Closed', value: res.data.closed },
            { label: 'Opened', value: res.data.opened },
          ];
          return transformedData;
        }),
        catchError((error) => {
          const customError = new Error(
            'Failed to get ticket status: ' + error.message
          );
          this._generalService.openSnackBar(customError.message);
          return throwError(() => {});
        })
      );
  }

  public getSupportTicketStats(id: string): Observable<Stat[]> {
    return this._httpClient
      .get<responseData>(
        `${this._apiUrl}/api/dashboard/support-ticket-stats/${id}`
      )
      .pipe(
        map((res) => {
          const transformedData = [
            { label: 'Closed', value: res.data.closed },
            { label: 'Assigned', value: res.data.notClosed },
          ];

          return transformedData;
        }),
        catchError((error) => {
          const customError = new Error(
            'Failed to get support tickets stats: ' + error.message
          );
          this._generalService.openSnackBar(customError.message);
          return throwError(() => {});
        })
      );
  }

  public getProductTicketStats(id: string): Observable<Stat[]> {
    return this._httpClient
      .get<responseData>(
        `${this._apiUrl}/api/dashboard/product-ticket-stats/${id}`
      )
      .pipe(
        map((res) => {
          const transformedData = [
            { label: 'Closed', value: res.data.closed },
            { label: 'Opened', value: res.data.notClosed },
          ];
          return transformedData;
        }),
        catchError((error) => {
          const customError = new Error(
            'Failed to get product ticket stats: ' + error.message
          );
          this._generalService.openSnackBar(customError.message);
          return throwError(() => {});
        })
      );
  }

  public getSummary(): Observable<
    {
      title: string;
      amount: number;
    }[]
  > {
    return this._httpClient
      .get<responseData>(`${this._apiUrl}/api/dashboard/summary`)
      .pipe(
        map((res) => {
          return res.data;
        }),
        catchError((error) => {
          const customError = new Error(
            'Failed to get product ticket stats: ' + error.message
          );
          this._generalService.openSnackBar(customError.message);
          return throwError(() => {});
        })
      );
  }

  chartsMap = new Map<string, Chart>();

  public createPieChart(
    data: any,
    id: string,
    type: keyof ChartTypeRegistry = 'pie'
  ): void {
    const ctx = (document.getElementById(id) as HTMLCanvasElement)?.getContext(
      '2d'
    );

    if (ctx) {
      const existingChart = this.chartsMap.get(id);
      if (existingChart) {
        existingChart.destroy();
      }

      const newChart = new Chart(ctx, {
        type,
        data: {
          labels: data.map((item: any) => item.label),
          datasets: [
            {
              data: data.map((item: any) => item.value),
              backgroundColor: [
                'rgba(255, 99, 132, 0.2)',
                'rgba(255, 159, 64, 0.2)',
                'rgba(255, 205, 86, 0.2)',
              ],

              borderColor: [
                'rgb(255, 99, 132)',
                'rgb(255, 159, 64)',
                'rgb(255, 205, 86)',
              ],
              borderWidth: 1,
            },
          ],
        },
        options: {
          responsive: true,
          maintainAspectRatio: false,
          plugins: {
            legend: {
              position: 'top',
            },
          },
        },
      });

      this.chartsMap.set(id, newChart);
    }
  }
}
