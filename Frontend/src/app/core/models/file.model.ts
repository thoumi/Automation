export interface FileUpload {
  id: number;
  fileName: string;
  originalFileName: string;
  filePath: string;
  fileType: string;
  fileSize: number;
  uploadedBy: string;
  uploadedAt: Date;
  processingStatus: FileProcessingStatus;
  processingMessage?: string;
}

export enum FileProcessingStatus {
  Pending = 0,
  Processing = 1,
  Completed = 2,
  Failed = 3
}

