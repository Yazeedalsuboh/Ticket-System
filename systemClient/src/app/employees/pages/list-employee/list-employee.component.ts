import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { ListUsersComponent } from '../../../shared/components/list-users/list-users.component';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-list-employee',
  imports: [RouterLink, RouterLinkActive, ListUsersComponent, MatButtonModule],
  templateUrl: './list-employee.component.html',
  styleUrl: './list-employee.component.css',
})
export default class ListEmployeeComponent {}
