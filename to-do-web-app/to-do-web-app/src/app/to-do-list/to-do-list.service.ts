import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { ToDoList } from '../models/to-do-list.model';
import { ToDoApiService } from '../core/to-do-api.service';

@Injectable({
  providedIn: 'root'
})
export class ToDoListService {
  private subject = new Subject<ToDoList>();

  constructor(private apiService: ToDoApiService) { }

  createToDoList() {
    this.apiService.createToDoList(new ToDoList()).subscribe({
      next: result => {
        this.subject.next(result);
      }
    });
  }

  get getToDoList() {
    return this.subject.asObservable();
  }
}
