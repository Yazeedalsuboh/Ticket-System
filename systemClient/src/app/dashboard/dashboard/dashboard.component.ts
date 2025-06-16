import { Component, signal } from '@angular/core';
import { TicketStatusComponent } from '../components/ticket-status/ticket-status.component';
import { SupportStatusComponent } from '../components/support-status/support-status.component';
import { SupportTicketStatusComponent } from '../components/support-ticket-status/support-ticket-status.component';
import { ProductTicketStatusComponent } from '../components/product-ticket-status/product-ticket-status.component';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { FormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';
import { SummaryComponent } from '../components/summary/summary.component';

@Component({
  selector: 'app-dashboard',
  imports: [
    TicketStatusComponent,
    SupportStatusComponent,
    SupportTicketStatusComponent,
    ProductTicketStatusComponent,
    MatButtonToggleModule,
    FormsModule,
    NgIf,
    SummaryComponent,
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css',
})
export default class DashboardComponent {
  selectedChart: string = 'ticket';
  chartType = signal<string>('pie');

  changeChart(e: any) {
    this.chartType.set(e);
  }
}
