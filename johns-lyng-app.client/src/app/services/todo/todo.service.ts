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

  baseUrl: string = 'https://localhost:44397/api/TodoItems';

  constructor(
    private http: HttpClient,
  ) {
    super();
  }

  getItems(): Observable<TodoItem[]> {
    return this.http.get<TodoItem[]>(this.baseUrl)
      .pipe(catchError(this.handleError));
  }

  addItem(description: string): Observable<any> {

    let todoItem: TodoItem = {
      id: uuidv4(),
      description: description,
      isCompleted: false
    }

    return this.http.post<any>(this.baseUrl, todoItem)
      .pipe(catchError(this.handleError));
  }

  markAsComplete(todoItem: TodoItem): Observable<any> {
    const updatedItem = { ...todoItem, isCompleted: true }; // Create a copy
    const url = `${this.baseUrl}/${todoItem.id}`;
    return this.http.put(url, updatedItem);
  }

}
