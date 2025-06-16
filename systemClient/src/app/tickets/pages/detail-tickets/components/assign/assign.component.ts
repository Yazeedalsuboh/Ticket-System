import { AsyncPipe } from '@angular/common';
import { Component, inject, input, OnInit, output } from '@angular/core';
import {
  FormBuilder,
  NonNullableFormBuilder,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { TranslatePipe } from '@ngx-translate/core';
import { Observable } from 'rxjs';
import { TicketsService } from '../../../../../shared/services/tickets.service';
import { MatButtonModule } from '@angular/material/button';
import { UsersService } from '../../../../../shared/services/users.service';
import { Employee } from '../../../../../shared/types/users-types';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-assign',
  imports: [
    MatFormFieldModule,
    AsyncPipe,
    MatSelectModule,
    ReactiveFormsModule,
    TranslatePipe,
    MatButtonModule,
    MatIconModule,
  ],
  templateUrl: './assign.component.html',
  styleUrl: './assign.component.css',
})
export class AssignComponent implements OnInit {
  private readonly _formBuilder = inject(FormBuilder);
  private readonly _ticketsService = inject(TicketsService);
  private readonly _usersService = inject(UsersService);

  employees$!: Observable<Employee[]>;
  ticketId = input<string>('');
  ticketReload = output();

  ngOnInit(): void {
    this.employees$ = this._usersService.getSupportUsers();
  }

  assignForm = this._formBuilder.group(
    {
      employeeId: ['', Validators.required],
      ticketId: ['' as string],
    },
    NonNullableFormBuilder
  );

  assign() {
    this.assignForm.get('ticketId')?.setValue(this.ticketId());

    this._ticketsService.assignTicket(this.assignForm.value).subscribe({
      next: (res) => {
        if (res.success) {
          this.ticketReload.emit();
        }
      },
    });
  }
}
