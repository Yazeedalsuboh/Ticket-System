import { Component } from '@angular/core';
import { ListUsersComponent } from '../../../shared/components/list-users/list-users.component';

@Component({
  selector: 'app-list-clients',
  imports: [ListUsersComponent],
  templateUrl: './list-clients.component.html',
  styleUrl: './list-clients.component.css',
})
export class ListClientsComponent {}
