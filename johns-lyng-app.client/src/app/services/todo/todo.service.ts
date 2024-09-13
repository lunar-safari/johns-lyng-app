import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { Observable } from "rxjs";
import { v4 as uuidv4 } from 'uuid';
import { TodoItem } from 'src/app/interfaces/app.interface';
import { BaseService } from 'src/app/base.service';

@Injectable({
  providedIn: 'root',
})

export class TodoDataService extends BaseService {

  baseUrl: string = '/api/TodoItems';

  constructor(
    private http: HttpClient,
  ) {
    super();
  }

  getItems(): Observable<TodoItem[]> {
    return this.http.get<TodoItem[]>('/api/TodoItems')
      .pipe(catchError(this.handleError));
  }

  addItem(description: string): Observable<any> {

    let todoItem: TodoItem = {
      id: uuidv4(),
      description: description,
      isCompleted: false
    }

    return this.http.post<any>('/api/TodoItems', todoItem)
      .pipe(catchError(this.handleError));
  }

  markAsComplete(todoItem: TodoItem): Observable<any> {
    const updatedItem = { ...todoItem, isCompleted: true }; // Create a copy
    const url = `/api/TodoItems/${todoItem.id}`;
    return this.http.put(url, updatedItem);
  }

}
