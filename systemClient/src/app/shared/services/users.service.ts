import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { catchError, map, Observable, throwError } from 'rxjs';
import { environment } from '../../../environments/environment.development';
import { GeneralService } from './general.service';
import { Employee, User } from '../types/users-types';
import { responseData, responseMessage } from '../types/responses-types';

@Injectable({
  providedIn: 'root',
})
export class UsersService {
  private readonly _apiUrl: string = environment.apiUrl;
  private readonly _httpClient = inject(HttpClient);
  private readonly _generalService = inject(GeneralService);

  public getSupportUsers(): Observable<Employee[]> {
    return this._httpClient
      .get<responseData<Employee[]>>(`${this._apiUrl}/api/users/support-only`)
      .pipe(
        map((response) => {
          return response.data;
        }),
        catchError((error) => {
          const customError = new Error(
            'Failed to get support users: ' + error.message
          );
          this._generalService.openSnackBar(customError.message);
          return throwError(() => {});
        })
      );
  }

  public getUsers0(formData: {
    pageNumber: number;
    pageSize: number;
    role?: any;
    search?: string;
  }): Observable<any> {
    return this._httpClient
      .post<responseData<any>>(
        `${this._apiUrl}/api/users/users-by-role`,
        formData
      )
      .pipe(
        map((response) => {
          return response.data;
        }),
        catchError((error) => {
          const customError = new Error(
            'Failed to get users by role: ' + error.message
          );
          this._generalService.openSnackBar(customError.message);
          return throwError(() => {});
        })
      );
  }

  public getSupportUsersAnalytics(): Observable<Employee[]> {
    return this._httpClient
      .get<responseData<Employee[]>>(`${this._apiUrl}/api/users/support`)
      .pipe(
        map((response) => {
          return response.data;
        }),
        catchError((error) => {
          const customError = new Error(
            'Failed to get support users: ' + error.message
          );
          this._generalService.openSnackBar(customError.message);
          return throwError(() => {});
        })
      );
  }

  public deactivateUser(id: string) {
    return this._httpClient
      .put<responseMessage>(`${this._apiUrl}/api/users/deactivate/${id}`, {})
      .pipe(
        map((response) => {
          this._generalService.openSnackBar(response.message);
          return { success: true };
        }),
        catchError((error) => {
          const customError = new Error(
            'Failed to deactivate user: ' + error.message
          );
          this._generalService.openSnackBar(customError.message);
          return throwError(() => {});
        })
      );
  }

  public activateUser(id: string) {
    return this._httpClient
      .put<responseMessage>(`${this._apiUrl}/api/users/activate/${id}`, {})
      .pipe(
        map((response) => {
          this._generalService.openSnackBar(response.message);
          return { success: true };
        }),
        catchError((error) => {
          const customError = new Error(
            'Failed to activate user: ' + error.message
          );
          this._generalService.openSnackBar(customError.message);
          return throwError(() => {});
        })
      );
  }
}
