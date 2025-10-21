export interface ScheduledTask {
  id: number;
  name: string;
  description: string;
  cronExpression: string;
  isEnabled: boolean;
  taskType: string;
  configuration?: string;
  lastExecutionTime?: Date;
  nextExecutionTime?: Date;
  createdAt: Date;
  updatedAt?: Date;
}

export interface TaskExecutionLog {
  id: number;
  taskName: string;
  executionTime: Date;
  status: TaskStatus;
  message?: string;
  errorDetails?: string;
  durationMs: number;
}

export enum TaskStatus {
  Success = 0,
  Failed = 1,
  Warning = 2,
  Running = 3
}

export interface TaskStats {
  totalExecutions: number;
  successCount: number;
  failedCount: number;
  warningCount: number;
  averageDurationMs: number;
  byTask: TaskStatsByName[];
  recentExecutions: TaskExecutionLog[];
}

export interface TaskStatsByName {
  taskName: string;
  count: number;
  successRate: number;
}

