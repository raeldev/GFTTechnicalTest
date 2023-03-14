import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { KanbanTask } from '../models/KanbanTask.model';

@Injectable({
  providedIn: 'root'
})
export class KanbanTaskService {
  readonly baseURL = "http://localhost:24288/api/KanbanTask";
  list: KanbanTask[]=[];

  constructor(private http: HttpClient) { }

  postKanbanTask(kanbanTask: KanbanTask) {
    return this.http.post(this.baseURL, kanbanTask, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    });
  }

  putKanbanTask(kanbanTask: KanbanTask) {
    return this.http.put(`${this.baseURL}/${kanbanTask.taskId}`, kanbanTask, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    });
  }

  deleteKanbanTask(taskId: number) {
    return this.http.delete(`${this.baseURL}/${taskId}`, {
      headers: new HttpHeaders({
        "Content-Type": "application/json"
      })
    });
  }

  refreshList() {
    this.http.get(this.baseURL)
    .toPromise()
    .then(res => {
      this.list = res as KanbanTask[]
    });
  }
}