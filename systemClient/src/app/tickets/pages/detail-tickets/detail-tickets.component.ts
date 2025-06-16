import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TicketsService } from '../../../shared/services/tickets.service';
import { MatCardModule } from '@angular/material/card';
import { AsyncPipe, CommonModule } from '@angular/common';
import { MatChipsModule } from '@angular/material/chips';
import { TranslatePipe } from '@ngx-translate/core';
import { MatButtonModule } from '@angular/material/button';
import { ApiService } from '../../../shared/services/api.service';
import { MatIconModule } from '@angular/material/icon';
import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { CommentComponent } from './components/comment/comment.component';
import { MatSelectModule } from '@angular/material/select';
import { AssignComponent } from './components/assign/assign.component';
import { Ticket, TicketStatus } from '../../../shared/types/tickets-types';
import { UserRoles } from '../../../shared/types/users-types';
import { Observable } from 'rxjs';
import { AiCohereService } from '../../../shared/services/ai-cohere.service';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
  selector: 'app-detail-tickets',
  imports: [
    MatCardModule,
    MatChipsModule,
    MatButtonModule,
    TranslatePipe,
    MatIconModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    CommonModule,
    MatInputModule,
    CommentComponent,
    MatSelectModule,
    ReactiveFormsModule,
    AssignComponent,
    AsyncPipe,
    MatTooltipModule,
  ],
  templateUrl: './detail-tickets.component.html',
  styleUrl: './detail-tickets.component.css',
})
export default class DetailTicketsComponent implements OnInit {
  readonly _ticketsService = inject(TicketsService);
  private readonly _router = inject(Router);
  private readonly _route = inject(ActivatedRoute);
  private readonly _apiService = inject(ApiService);
  readonly _aiService = inject(AiCohereService);

  translatedText = signal({
    title: '',
    description: '',
  });

  TicketStatus = TicketStatus;
  UserRoles = UserRoles;

  profile = computed(() => this._apiService.profile());
  ticket$!: Observable<Ticket>;
  ticketId = signal<string>('');

  ngOnInit(): void {
    this.ticketId.set(this._route.snapshot.paramMap.get('id')!);
    this.reloadTicket();

    this.ticket$.subscribe((ticket) => {
      if (
        this.profile().role == UserRoles.Client &&
        ticket.status == TicketStatus.Open &&
        ticket.priority == null
      ) {
        this._aiService.determinePriority(ticket, this.ticketId());
      }
    });
  }

  reloadTicket() {
    this.ticket$ = this._ticketsService.byId(this.ticketId());
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const newFile = input.files[0];
      this._ticketsService.addImage(newFile, this.ticketId()).subscribe({
        next: (res) => {
          if (res.success) {
            this.reloadTicket();
          }
        },
      });
    }
  }

  closeTicket() {
    const confirmed = window.confirm(
      'Are you sure you want to close this ticket?'
    );
    if (confirmed) {
      this._ticketsService.closeTicket(this.ticketId()).subscribe({
        next: (res) => {
          if (res.success) {
            this.reloadTicket();
            this._router.navigate([`/tickets/list`]);
          }
        },
      });
    }
  }

  update() {
    this._ticketsService.toUpdateId.set(this.ticketId());
    this._router.navigate(['tickets/add']);
  }

  translateText(text: string, key: string) {
    this._aiService
      .translateText(text)
      .then((reply) => {
        this.ticket$.subscribe((ticket) => {
          if (key === 'title') {
            this.translatedText().title = reply;
          } else {
            this.translatedText().description = reply;
          }
        });
      })
      .catch((error) => {
        console.error(error);
      });
  }
}
