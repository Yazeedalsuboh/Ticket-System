import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, switchMap, throwError } from 'rxjs';
import { ApiService } from '../services/api.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const apiService = inject(ApiService);

  return next(req).pipe(
    catchError((err) => {
      if (err.status === 401) {
        return apiService.refreshToken().pipe(
          switchMap((response) => {
            const newToken = response.data.token;
            const clonedReq = req.clone({
              setHeaders: {
                Authorization: `Bearer ${newToken}`,
              },
            });
            return next(clonedReq);
          }),
          catchError((refreshError) => {
            return throwError(() => refreshError);
          })
        );
      }
      return throwError(() => err);
    })
  );
};
