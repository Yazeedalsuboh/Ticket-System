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
import { TranslatePipe } from '@ngx-translate/core';
import { ApiService } from '../../../shared/services/api.service';

@Component({
  selector: 'app-change-password',
  imports: [
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatButtonModule,
    TranslatePipe,
    ReactiveFormsModule,
    NgIf,
  ],
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.css',
})
export default class ChangePasswordComponent {
  private readonly _formBuilder = inject(FormBuilder);
  private readonly _apiService = inject(ApiService);

  showNewPassword = false;
  showCurrentPassword = false;

  toggleNewPassword() {
    this.showNewPassword = !this.showNewPassword;
  }

  toggleCurrentPassword() {
    this.showCurrentPassword = !this.showCurrentPassword;
  }

  Form = this._formBuilder.group(
    {
      currentPassword: ['', [Validators.required]],
      newPassword: [
        '',
        [
          Validators.required,
          Validators.pattern(
            '(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[$@$!%*?&])[A-Za-zd$@$!%*?&].{8,}'
          ),
        ],
      ],
    },
    NonNullableFormBuilder
  );

  submit() {
    this._apiService.changePassword(this.Form.value).subscribe();
  }
}
