import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { TodoItem } from 'src/app/interfaces/app.interface';
import { TodoDataService } from 'src/app/services/todo/todo.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  items: TodoItem[] = [];
  completedItems: TodoItem[] = [];
  description: string = '';
  errors: string = '';

  constructor(private todoService: TodoDataService) {}

  ngOnInit() {
    this.getItems();
  }

  getItems() {
    this.clearError();
    const getItemsObserver = {
      next: (result: any) => {
        this.items = result.filter(x => !x.isCompleted);
        this.completedItems = result.filter(x => x.isCompleted);
      },
      error: (err: any) => { this.errors = err; }
    };
    this.todoService.getItems().subscribe(getItemsObserver);
  }

  addItem() {
    this.clearError();
    const addItemObserver = {
      next: (result: any) => {
        this.getItems();
        this.handleClear();
      },
      error: (err: any) => { this.errors = err; }
    };
    this.todoService.addItem(this.description).subscribe(addItemObserver);
  }

  markAsComplete(item: TodoItem): void {
    this.clearError();
    const markAsCompleteObserver = {
      next: (result: any) => {
        this.getItems();
      },
      error: (err: any) => { this.errors = err; }
    };
    this.todoService.markAsComplete(item).subscribe(markAsCompleteObserver);
  }


  handleClear() {
    this.description = '';
  } 

  clearError() {
    this.errors = '';
  }

  title = 'johns-lyng-app.client';
}
