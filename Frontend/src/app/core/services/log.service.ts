import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { TaskExecutionLog, TaskStats, TaskStatus } from '../models/task.model';

export interface LogsResponse {
  data: TaskExecutionLog[];
  pagination: {
    page: number;
    pageSize: number;
    total: number;
    totalPages: number;
  };
}

export interface LogsFilter {
  taskName?: string;
  startDate?: Date;
  endDate?: Date;
  status?: TaskStatus;
  page?: number;
  pageSize?: number;
}

@Injectable({
  providedIn: 'root'
})
export class LogService {
  constructor(private api: ApiService) {}

  getLogs(filter: LogsFilter = {}): Observable<LogsResponse> {
    return this.api.get<LogsResponse>('logs', filter);
  }

  getLog(id: number): Observable<TaskExecutionLog> {
    return this.api.get<TaskExecutionLog>(`logs/${id}`);
  }

  getStats(days: number = 7): Observable<TaskStats> {
    return this.api.get<TaskStats>('logs/stats', { days });
  }

  cleanupOldLogs(daysToKeep: number = 30): Observable<{ deletedCount: number }> {
    return this.api.delete<{ deletedCount: number }>(`logs/cleanup?daysToKeep=${daysToKeep}`);
  }
}

