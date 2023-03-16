import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { KanbanTask } from '../models/KanbanTask.model';

@Injectable({
  providedIn: 'root'
})
export class KanbanTaskService {
  readonly baseURL = "http://localhost:5020/tasks";

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

  async refreshList() : Promise<KanbanTask[]> {
    const res = await this.http.get(`${this.baseURL}/all`)
      .toPromise();
    console.log(res);
    return res as KanbanTask[];
  }
}