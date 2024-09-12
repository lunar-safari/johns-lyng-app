import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export abstract class BaseService {

  constructor() { }

  public handleError(error: HttpErrorResponse): Observable<never> {

    console.log('************ ERROR START *************');
    console.log(error);
    console.log('************ ERROR END *************');

    if (error.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      console.error('An error occurred:', error.error.message);
      return throwError(() => new Error('Something has gone wrong on the client.<br/>' + error.error.message));

    } else {
      // The backend returned an unsuccessful response code.
      console.error(`Backend returned code ${error.status}, body was: ${error.error}`);
      return throwError(() => new Error('Something has gone wrong on the server.<br/><br/>' + error.error + '<br/>' + error.message));
    }

  }
}
