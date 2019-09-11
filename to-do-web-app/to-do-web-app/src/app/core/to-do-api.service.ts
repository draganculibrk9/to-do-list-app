import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { ToDoList } from '../models/to-do-list.model';
import { ToDoItem } from '../models/to-do-item.model';

@Injectable({
  providedIn: 'root'
})
export class ToDoApiService {
  private http: HttpClient;
  baseUrl: string;

  constructor(httpClient: HttpClient) {
    this.http = httpClient;
    this.baseUrl = environment.baseURL;
  }

  getToDoLists(): Observable<ToDoList[]> {
    return this.http.get(`${this.baseUrl}/to-do-lists`).pipe(
      map((data: ToDoList[]) => {
        return data;
      })
    );
  }

  getToDoList(id: string): Observable<ToDoList> {
    return this.http.get(`${this.baseUrl}/to-do-lists/${id}`).pipe(
      map((data: ToDoList) => {
        return data;
      })
    );
  }

  deleteToDoList(id: string) {
    return this.http.delete(`${this.baseUrl}/to-do-lists/${id}`)
  }

  editToDoList(list: ToDoList) {
    return this.http.put<ToDoList>(`${this.baseUrl}/to-do-lists`, list);
  }

  editToDoItem(listId: string, item: ToDoItem) {
    return this.http.put<ToDoItem>(`${this.baseUrl}/to-do-lists/${listId}/to-do-items`, item);
  }

  getToDoItem(listId: string, itemId: string) {
    return this.http.get(`${this.baseUrl}/to-do-lists/${listId}/to-do-items/${itemId}`);
  }

  getToDoItems(listId: string): Observable<ToDoItem[]> {
    return this.http.get(`${this.baseUrl}/to-do-lists/${listId}/to-do-items`).pipe(
      map((data: ToDoItem[]) => {
        return data;
      })
    )
  }

  removeToDoItem(listId: string, itemId: string) {
    return this.http.delete(`${this.baseUrl}/to-do-lists/${listId}/to-do-items/${itemId}`);
  }

  createToDoItem(listId: string, item: ToDoItem) {
    return this.http.post<ToDoItem>(`${this.baseUrl}/to-do-lists/${listId}/to-do-items`, item);
  }

  createToDoList(list: ToDoList) {
    return this.http.post<ToDoList>(`${this.baseUrl}/to-do-lists`, list);
  }

  updateToDoItemPosition(listId: string, itemId: string, position: number) {
    return this.http.put(`${this.baseUrl}/to-do-lists/${listId}/to-do-items/${itemId}/position/${position}`, null);
  }

  updateToDoListPosition(listId: string, position: number) {
    return this.http.put(`${this.baseUrl}/to-do-lists/${listId}/position/${position}`, null);
  }

  search(title: string) {
    let params = new HttpParams().set('title', title);
    return this.http.get(`${this.baseUrl}/to-do-lists/search`, { params: params });
  }

  getToDoListShare(shareId: string): Observable<ToDoList> {
    return this.http.get(`${this.baseUrl}/to-do-lists/to-do-list-shares/${shareId}`).pipe(
      map((data: ToDoList) => {
        return data;
      })
    );
  }

  createToDoListShare(listId: string): Observable<string> {
    return this.http.post<string>(`${this.baseUrl}/to-do-lists/${listId}/to-do-list-shares`, {}).pipe(
      map((data: string) => {
        return data;
      })
    );
  }
}
