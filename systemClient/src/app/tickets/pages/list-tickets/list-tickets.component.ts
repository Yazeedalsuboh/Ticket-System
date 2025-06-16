import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { Observable } from 'rxjs';
import { TicketsService } from '../../../shared/services/tickets.service';
import { TranslatePipe } from '@ngx-translate/core';
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ticketBrief, TicketStatus } from '../../../shared/types/tickets-types';
import { MatPaginatorModule } from '@angular/material/paginator';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatInput, MatInputModule } from '@angular/material/input';
import { Product } from '../../../shared/types/products-types';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { UsersService } from '../../../shared/services/users.service';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatIconModule } from '@angular/material/icon';
import { ApiService } from '../../../shared/services/api.service';
import { UserRoles } from '../../../shared/types/users-types';

@Component({
  selector: 'app-list-tickets',
  imports: [
    TranslatePipe,
    MatSelectModule,
    MatTableModule,
    RouterLink,
    MatPaginatorModule,
    CommonModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatInputModule,
    MatCheckboxModule,
    MatSidenavModule,
    MatInput,
    MatIconModule,
  ],
  templateUrl: './list-tickets.component.html',
  styleUrl: './list-tickets.component.css',
})
export default class ListTicketsComponent implements OnInit {
  private readonly _ticketsService = inject(TicketsService);
  private readonly _usersService = inject(UsersService);
  private readonly _route = inject(ActivatedRoute);
  private readonly _formBuilder = inject(FormBuilder);
  readonly _apiService = inject(ApiService);

  profile = computed(() => this._apiService.profile());
  UserRoles = UserRoles;

  ticketStatuses = Object.values(TicketStatus);
  products$!: Observable<Product[]>;
  users$!: Observable<{
    result: any[];
    pageSize: number;
    pageNumber: number;
    totalCount: number;
  }>;

  data$!: Observable<{
    tickets: any[];
    pageSize: number;
    pageNumber: number;
    totalCount: number;
  }>;
  pagination = signal({
    pageSize: 10,
    pageIndex: 0,
  });
  id: string | null = null;

  Form = this._formBuilder.group(
    {
      status: [''],
      searchTitle: [''],
      productId: [''],
      userIdFilter: [''],
      userRoleFilter: [''],
      sortByCreatedAtAsc: [false],
    },
    { NonNullableFormBuilder: true }
  );

  ngOnInit(): void {
    this.products$ = this._ticketsService.getProducts();
    this.users$ = this._usersService.getUsers0({
      pageNumber: 0,
      pageSize: 10,
    });

    this._route.queryParamMap.subscribe((params) => {
      this.id = params.get('id');

      if (this.id) {
        this.Form.get('userIdFilter')?.setValue(this.id);
      }
    });

    this.loadTickets();

    this.Form.valueChanges.subscribe(() => {
      this.submitForm();
    });
  }

  displayedColumns: string[] = [
    'title',
    'status',
    'client',
    'product',
    'assigned',
    'created at',
    'priority',
  ];

  onPageChange(e: any) {
    this.pagination.update((val) => ({
      ...val,
      pageIndex: e.pageIndex,
      pageSize: e.pageSize,
    }));
    this.loadTickets();
  }

  private loadTickets() {
    this.data$ = this._ticketsService.filter(
      this.pagination(),
      this.Form.value
    );
  }

  submitForm() {
    this.pagination.update(() => ({
      pageIndex: 0,
      pageSize: 10,
    }));
    this.loadTickets();
  }
}
