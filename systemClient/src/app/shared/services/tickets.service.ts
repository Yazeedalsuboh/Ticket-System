import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { catchError, map, Observable, throwError } from 'rxjs';
import { addTicket } from '../types/auth-types';
import { GeneralService } from './general.service';
import { Product } from '../types/products-types';
import { responseData, responseMessage } from '../types/responses-types';
import {
  AddComment,
  AssignTicket,
  Ticket,
  ticketBrief,
} from '../types/tickets-types';

@Injectable({
  providedIn: 'root',
})
export class TicketsService {
  private readonly _apiUrl: string = environment.apiUrl;

  // Injections
  private readonly _httpClient = inject(HttpClient);
  private readonly _generalService = inject(GeneralService);

  toUpdateId = signal<string>('');

  // Methods
  public add(data: addTicket) {
    const formData = new FormData();

    formData.append('title', data.title!);
    formData.append('description', data.description!);
    formData.append('productName', data.productName!);

    for (const file of data.attachments ?? []) {
      formData.append('attachments', file);
    }

    return this._httpClient
      .post<responseMessage>(`${this._apiUrl}/api/ticket/add-ticket`, formData)
      .pipe(
        map((response) => {
          this._generalService.openSnackBar(response.message);
          return { success: true };
        }),
        catchError((error) => {
          const customError = new Error(
            'Failed to add ticket: ' + error.message
          );
          this._generalService.openSnackBar(customError.message);
          return throwError(() => {});
        })
      );
  }

  public getProducts(): Observable<Product[]> {
    return this._httpClient
      .get<responseData<Product[]>>(`${this._apiUrl}/api/product/product-list`)
      .pipe(
        map((response) => {
          return response.data;
        }),
        catchError((error) => {
          const customError = new Error(
            'Failed to fetch products: ' + error.message
          );
          this._generalService.openSnackBar(customError.message);
          return throwError(() => {});
        })
      );
  }
  public filter(
    pagination: {
      pageIndex: number;
      pageSize: number;
    },
    formData: any
  ): Observable<any> {
    let status = formData.status;
    if (status === 'All') status = '';
    if (status === 'Assign') status = 1;
    if (status === 'Open') status = 0;
    if (status === 'Closed') status = 2;

    const requestBody: any = {
      pageNumber: pagination.pageIndex,
      pageSize: pagination.pageSize,
    };

    if (formData.searchTitle?.trim())
      requestBody.searchTitle = formData.searchTitle;
    if (formData.productId !== null && formData.productId !== '')
      requestBody.productId = formData.productId;
    if (
      formData.sortByCreatedAtAsc === true ||
      formData.sortByCreatedAtAsc === false
    )
      requestBody.sortByCreatedAtAsc = formData.sortByCreatedAtAsc;
    if (formData.userIdFilter !== null && formData.userIdFilter !== '')
      requestBody.userIdFilter = formData.userIdFilter;
    if (status !== '' && status !== undefined && status !== null)
      requestBody.status = status;

    return this._httpClient
      .post<responseData<any>>(
        `${this._apiUrl}/api/ticket/filter-ticket-list`,
        requestBody
      )
      .pipe(
        map((response) => response.data),
        catchError((error) => {
          const customError = new Error(
            'Failed to filter tickets: ' + error.message
          );
          this._generalService.openSnackBar(customError.message);
          return throwError(() => {});
        })
      );
  }

  public byId(id: string): Observable<Ticket> {
    return this._httpClient
      .get<responseData<Ticket>>(`${this._apiUrl}/api/ticket/${id}`)
      .pipe(
        map((response) => {
          return response.data;
        }),
        catchError((error) => {
          const customError = new Error(
            'Failed to get ticket by ID: ' + error.message
          );
          this._generalService.openSnackBar(customError.message);
          return throwError(() => {});
        })
      );
  }

  public addComment(data: AddComment) {
    return this._httpClient
      .post<responseMessage>(`${this._apiUrl}/api/comment/add-comment`, data)
      .pipe(
        map((response) => {
          return { success: true };
        }),
        catchError((error) => {
          const customError = new Error(
            'Failed to add comment: ' + error.message
          );
          this._generalService.openSnackBar(customError.message);
          return throwError(() => {});
        })
      );
  }

  public addImage(image: File, id: string) {
    const formData = new FormData();

    formData.append('id', id!);
    formData.append('files', image!);

    return this._httpClient
      .post<responseMessage>(`${this._apiUrl}/api/ticket/attachments`, formData)
      .pipe(
        map((response) => {
          return { success: true };
        }),
        catchError((error) => {
          const customError = new Error(
            'Failed to add image: ' + error.message
          );
          this._generalService.openSnackBar(customError.message);
          return throwError(() => {});
        })
      );
  }

  public update(data: addTicket) {
    const formData = new FormData();

    formData.append('title', data.title);
    formData.append('description', data.description);
    formData.append('productName', data.productName);
    formData.append('priority', data.priority);
    formData.append('id', this.toUpdateId());

    for (const file of data.attachments ?? []) {
      formData.append('attachments', file);
    }

    return this._httpClient
      .put<responseMessage>(
        `${this._apiUrl}/api/ticket/update-ticket`,
        formData
      )
      .pipe(
        map((response) => {
          this._generalService.openSnackBar(response.message);
          this.toUpdateId.set(''); //
          return { success: true };
        }),
        catchError((error) => {
          const customError = new Error(
            'Failed to update ticket: ' + error.message
          );
          this._generalService.openSnackBar(customError.message);
          return throwError(() => {});
        })
      );
  }

  public assignTicket(assigndata: AssignTicket) {
    return this._httpClient
      .post<responseMessage>(`${this._apiUrl}/api/ticket/assign`, assigndata)
      .pipe(
        map((response) => {
          this._generalService.openSnackBar(response.message);
          return { success: true };
        }),
        catchError((error) => {
          const customError = new Error(
            'Failed to assign ticket: ' + error.message
          );
          this._generalService.openSnackBar(customError.message);
          return throwError(() => {});
        })
      );
  }

  public closeTicket(id: string) {
    return this._httpClient
      .put<responseMessage>(`${this._apiUrl}/api/ticket/close-ticket/${id}`, {})
      .pipe(
        map((response) => {
          this._generalService.openSnackBar(response.message);
          return { success: true };
        }),
        catchError((error) => {
          const customError = new Error(
            'Failed to close ticket: ' + error.message
          );
          this._generalService.openSnackBar(customError.message);
          return throwError(() => {});
        })
      );
  }
}
