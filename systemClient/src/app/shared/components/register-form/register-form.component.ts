import { NgIf } from '@angular/common';
import { Component, inject, input, output } from '@angular/core';
import {
  FormBuilder,
  NonNullableFormBuilder,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { RouterLink } from '@angular/router';
import { TranslatePipe } from '@ngx-translate/core';
import { format } from 'date-fns';
import { register } from '../../types/auth-types';

@Component({
  selector: 'app-register-form',
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatGridListModule,
    NgIf,
    TranslatePipe,
    RouterLink,
  ],
  templateUrl: './register-form.component.html',
  styleUrl: './register-form.component.css',
})
export class RegisterFormComponent {
  readonly formBuilder = inject(FormBuilder);

  readonly formData = output<register>();
  readonly signup = input<Boolean>();

  photoPreviewUrl: string | null = null;

  showPassword = false;

  togglePassword() {
    this.showPassword = !this.showPassword;
  }

  Form = this.formBuilder.group(
    {
      fullName: ['', [Validators.required, Validators.maxLength(20)]],
      email: ['', [Validators.required, Validators.email]],
      password: [
        '',
        [
          Validators.pattern(
            '(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[$@$!%*?&])[A-Za-zd$@$!%*?&].{8,}'
          ),
        ],
      ],
      mobile: ['', [Validators.required, Validators.pattern(/^07[7-9]\d{7}$/)]],
      dateOfBirth: ['', Validators.required],
      address: ['', [Validators.required, Validators.maxLength(20)]],
      image: [''],
    },
    NonNullableFormBuilder
  );

  submit() {
    const rawValue = this.Form.get('dateOfBirth')?.value as string;
    const formattedDate = format(rawValue, 'yyyy-MM-dd');
    this.Form.get('dateOfBirth')?.setValue(formattedDate);
    this.formData.emit(this.Form.value);
  }

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      if (file.type.startsWith('image/')) {
        const reader = new FileReader();
        reader.onload = () => {
          const base64Image = reader.result as string;
          this.Form.controls['image'].setValue(base64Image);
          this.photoPreviewUrl = base64Image;
        };
        reader.readAsDataURL(file);
      }
    }
  }
}
