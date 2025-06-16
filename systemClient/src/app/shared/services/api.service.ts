import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
} from '@angular/common/http';
import { catchError, map, throwError } from 'rxjs';
import {
  login,
  Profile,
  register,
  Token,
  TokenResponse,
} from '../types/auth-types';
import { GeneralService } from './general.service';
import { responseData, responseMessage } from '../types/responses-types';
import { jwtDecode } from 'jwt-decode';
import { Router } from '@angular/router';
import { BadRequestError, UnauthorizedError } from 'cohere-ai/api';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  private apiUrl: string = environment.apiUrl;
  private readonly _generalService = inject(GeneralService);
  private readonly _router = inject(Router);
  private readonly _httpClient = inject(HttpClient);

  role = signal<string>('');
  currentLang = signal<string>('en');

  profile = signal<Profile>({
    role: '',
    name: '',
    email: '',
  });

  public refreshProfile() {
    this.profile.set({
      role: localStorage.getItem('role') ?? '',
      name: localStorage.getItem('name') ?? '',
      email: localStorage.getItem('email') ?? '',
    });
  }

  public storeInLS(response: responseData<TokenResponse>) {
    localStorage.setItem('access_token', response.data.token);
    localStorage.setItem('refresh_token', response.data.refreshToken);

    const decodedToken: any = jwtDecode(response.data.token);

    localStorage.setItem(
      'role',
      decodedToken[
        'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
      ]
    );
    localStorage.setItem('name', decodedToken['unique_name']);
    localStorage.setItem('email', decodedToken['email']);

    this.refreshProfile();
    this.role.set(
      decodedToken[
        'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
      ]
    );

    if (this.role() === 'Client' || this.role() === 'Support') {
      this._router.navigate(['/tickets/list']);
    } else if (this.role() === 'Manager') {
      this._router.navigate(['/dashboard']);
    }
  }

  public signup(formData: register) {
    return this._httpClient
      .post<responseMessage>(
        `${this.apiUrl}/api/authentication/client-register`,
        formData
      )
      .pipe(
        map((response) => {
          this._generalService.openSnackBar(response.message);
          return { success: true };
        }),
        catchError((error) => {
          const customError = new Error('Failed to signup: ' + error.message);
          this._generalService.openSnackBar(customError.message);
          return throwError(() => {});
        })
      );
  }

  public login(formData: login) {
    return this._httpClient
      .post<responseData<Token>>(
        `${this.apiUrl}/api/authentication/login`,
        formData
      )
      .pipe(
        map((response) => {
          this.storeInLS(response);

          return { success: true };
        }),
        catchError((error: HttpErrorResponse) => {
          console.error(error);
          const customError = new Error('Failed Login: ');
          this._generalService.openSnackBar(customError.message);
          return throwError(() => {});
        })
      );
  }

  public registerEmployee(formData: register) {
    return this._httpClient
      .post<responseMessage>(
        `${this.apiUrl}/api/authentication/support-register`,
        formData
      )
      .pipe(
        map((response) => {
          this._generalService.openSnackBar(response.message);
          return { success: true };
        }),
        catchError((error) => {
          const customError = new Error(
            'Failed to register employee',
            error.message
          );
          this._generalService.openSnackBar(customError.message);
          return throwError(() => {});
        })
      );
  }

  public refreshToken() {
    let refreshToken = localStorage.getItem('refresh_token');
    return this._httpClient
      .post<responseData<TokenResponse>>(
        `${this.apiUrl}/api/authentication/refresh`,
        { refreshToken }
      )
      .pipe(
        map((response) => {
          this.storeInLS(response);
          return { success: true, data: response.data };
        }),
        catchError((error) => {
          const customError = new Error(
            'Failed refresh token: ' + error.message
          );
          this._generalService.openSnackBar(customError.message);
          return throwError(() => {});
        })
      );
  }

  public logout() {
    localStorage.clear();
    this._router.navigate(['/auth/login']);
    this.profile.set({ name: '', email: '', role: '' });
    this.role.set('');
  }

  public changePassword(formData: {
    currentPassword: number;
    newPassword: number;
  }) {
    return this._httpClient
      .post<responseMessage>(
        `${this.apiUrl}/api/authentication/change-password`,
        formData
      )
      .pipe(
        map((response) => {
          this._generalService.openSnackBar(response.message);
          this._router.navigate(['/tickets/list']);
          return { success: true };
        }),
        catchError(() => {
          const customError = new Error('Changing password failed!');
          this._generalService.openSnackBar(customError.message);
          return throwError(() => {});
        })
      );
  }

  public forgotPassword(formData: { email: string }) {
    return this._httpClient
      .post<responseMessage>(
        `${this.apiUrl}/api/authentication/send-verification-code`,
        formData
      )
      .pipe(
        map((response) => {
          this._generalService.openSnackBar(response.message);
          return { success: true };
        }),
        catchError(() => {
          const customError = new Error('Changing password failed!');
          this._generalService.openSnackBar(customError.message);
          return throwError(() => {});
        })
      );
  }

  public verifyCode(formData: { email: string; code: string }) {
    return this._httpClient
      .post<responseMessage>(
        `${this.apiUrl}/api/authentication/verify-code`,
        formData
      )
      .pipe(
        map((response) => {
          this._generalService.openSnackBar(response.message);
          return { success: true };
        }),
        catchError(() => {
          const customError = new Error('Changing password failed!');
          this._generalService.openSnackBar(customError.message);
          return throwError(() => {});
        })
      );
  }

  public updatePassword(formData: { email: string; password: string }) {
    return this._httpClient
      .post<responseMessage>(
        `${this.apiUrl}/api/authentication/forgot-password`,
        formData
      )
      .pipe(
        map((response) => {
          this._generalService.openSnackBar(response.message);
          return { success: true };
        }),
        catchError(() => {
          const customError = new Error('Changing password failed!');
          this._generalService.openSnackBar(customError.message);
          return throwError(() => {});
        })
      );
  }
}
