import { NgIf } from '@angular/common';
import { Component, inject } from '@angular/core';
import {
  FormBuilder,
  NonNullableFormBuilder,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { TranslatePipe } from '@ngx-translate/core';
import { ApiService } from '../../../shared/services/api.service';

@Component({
  selector: 'app-login',
  imports: [
    RouterLink,
    RouterLinkActive,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatButtonModule,
    TranslatePipe,
    ReactiveFormsModule,
    NgIf,
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export default class LoginComponent {
  private readonly _formBuilder = inject(FormBuilder);
  private readonly _apiService = inject(ApiService);
  showPassword = false;

  togglePassword() {
    this.showPassword = !this.showPassword;
  }

  Form = this._formBuilder.group(
    {
      identifier: ['', [Validators.required]],
      password: ['', [Validators.required]],
    },
    NonNullableFormBuilder
  );

  submit() {
    this._apiService.login(this.Form.value).subscribe();
  }
}
