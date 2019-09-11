import { Injectable } from '@angular/core';
import { ToDoList } from './models/to-do-list.model';
import { Subject } from 'rxjs';
import { ToDoApiService } from './core/to-do-api.service';

@Injectable({
  providedIn: 'root'
})
export class SearchService {
  private subject = new Subject<ToDoList[]>();

  constructor(private apiService: ToDoApiService) { }

  search(title: string) {
    if (title === "") {
      this.apiService.getToDoLists().subscribe({
        next: result => {
          this.subject.next(result);
        }
      })
    } else {
      this.apiService.search(title).subscribe({
        next: (result: ToDoList[]) => {
          this.subject.next(result);
        }
      });
    }
  }

  getSearchResults() {
    return this.subject.asObservable();
  }

}
