import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { ScheduledTask } from '../models/task.model';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  constructor(private api: ApiService) {}

  getTasks(): Observable<ScheduledTask[]> {
    return this.api.get<ScheduledTask[]>('tasks');
  }

  getTask(id: number): Observable<ScheduledTask> {
    return this.api.get<ScheduledTask>(`tasks/${id}`);
  }

  createTask(task: Partial<ScheduledTask>): Observable<ScheduledTask> {
    return this.api.post<ScheduledTask>('tasks', task);
  }

  updateTask(id: number, task: ScheduledTask): Observable<void> {
    return this.api.put<void>(`tasks/${id}`, task);
  }

  deleteTask(id: number): Observable<void> {
    return this.api.delete<void>(`tasks/${id}`);
  }

  executeTask(id: number): Observable<{ jobId: string }> {
    return this.api.post<{ jobId: string }>(`tasks/${id}/execute`, {});
  }
}

