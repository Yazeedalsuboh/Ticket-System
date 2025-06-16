import { CommonModule } from '@angular/common';
import { Component, computed, inject, input, output } from '@angular/core';
import {
  FormBuilder,
  NonNullableFormBuilder,
  ReactiveFormsModule,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormField } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { TranslatePipe } from '@ngx-translate/core';
import { TicketsService } from '../../../../../shared/services/tickets.service';
import { ApiService } from '../../../../../shared/services/api.service';
import {
  Ticket,
  TicketStatus,
} from '../../../../../shared/types/tickets-types';
import { UserRoles } from '../../../../../shared/types/users-types';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-comment',
  imports: [
    ReactiveFormsModule,
    CommonModule,
    MatFormField,
    MatInputModule,
    MatButtonModule,
    TranslatePipe,
    MatIconModule,
  ],
  templateUrl: './comment.component.html',
  styleUrl: './comment.component.css',
})
export class CommentComponent {
  private readonly _formBuilder = inject(FormBuilder);
  readonly ticketsService = inject(TicketsService);
  readonly apiService = inject(ApiService);
  profile = computed(() => this.apiService.profile());

  ticketId = input<string>('');
  ticket = input<Ticket>();
  ticketReload = output();

  TicketStatus = TicketStatus;
  UserRoles = UserRoles;

  commentForm = this._formBuilder.group(
    {
      id: [''],
      message: [''],
    },
    NonNullableFormBuilder
  );

  submitComment() {
    this.commentForm.get('id')?.setValue(this.ticketId());

    this.ticketsService.addComment(this.commentForm.value).subscribe({
      next: (res) => {
        if (res.success) {
          this.ticketReload.emit();
          this.commentForm.reset();
        }
      },
    });
  }
}
