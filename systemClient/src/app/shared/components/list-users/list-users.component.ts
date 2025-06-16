import { Component, inject, input, signal } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { UsersService } from '../../services/users.service';
import { User } from '../../types/users-types';
import { TranslatePipe } from '@ngx-translate/core';
import { MatInput } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { Observable } from 'rxjs';
import { AsyncPipe, NgIf } from '@angular/common';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-list-users',
  imports: [
    MatTableModule,
    MatSlideToggleModule,
    TranslatePipe,
    MatInput,
    MatFormFieldModule,
    ReactiveFormsModule,
    MatButtonModule,
    NgIf,
    AsyncPipe,
    MatPaginatorModule,
    MatIconModule,
    RouterLink,
  ],
  templateUrl: './list-users.component.html',
  styleUrl: './list-users.component.css',
})
export class ListUsersComponent {
  private readonly _usersService = inject(UsersService);
  filter = input<string>('');
  pagination = signal({
    pageIndex: 0,
    pageSize: 10,
  });
  users$!: Observable<{
    result: any[];
    pageSize: number;
    pageNumber: number;
    totalCount: number;
  }>;
  private readonly _formBuilder = inject(FormBuilder);
  Form = this._formBuilder.group(
    {
      search: [''],
    },
    { NonNullableFormBuilder: true }
  );

  submit() {
    this.pagination.update((val) => ({
      ...val,
      pageIndex: 0,
    }));
    this.loadUserS();
  }

  ngOnInit(): void {
    this.loadUserS();
  }

  loadUserS(e?: any) {
    if (e) {
      this.pagination.set({
        pageIndex: e.pageIndex,
        pageSize: e.pageSize,
      });
    }

    const { pageIndex, pageSize } = this.pagination();

    this.users$ = this._usersService.getUsers0({
      pageNumber: pageIndex,
      pageSize: pageSize,
      search: this.Form.get('search')?.value,
      role: this.filter() === 'Client' ? 0 : 1,
    });
  }
  displayedColumns: string[] = [
    'Full Name',
    'Mobile',
    'Email',
    'Ticket Count',
    'Active',
  ];

  toggle(isCurrentlyActive: boolean, id: string) {
    const toggleCall = isCurrentlyActive
      ? this._usersService.deactivateUser(id)
      : this._usersService.activateUser(id);

    toggleCall.subscribe({
      next: (res) => {
        if (res.success) {
          this.loadUserS();
        }
      },
      error: (err) => {
        console.error('Toggle failed:', err);
      },
    });
  }
}
