export enum SearchStateEnum {
  Loading = 'loading',
  Finished = 'finished',
  Error = 'error'
}

export interface ApiResponse {
  state: SearchStateEnum;
  code: number;
  data: any;
  message: string;
}

export interface SearchResults {
  term: string;
  references: Reference[];
}

export interface Reference {
  tittle: string;
  occurrences: number;
  url: string;
}

export enum StatusEnum {
  None = 'none',
  ScheduledIndex = 'scheduledIndex',
  ScheduledDelete = 'scheduledDelete',
  Indexed = 'indexed',
  Failed = 'failed',
  Deleted = 'deleted'
}

export interface DocumentStatus {
  id: number;
  filename: string;
  title: string;
  isOcrRequested: boolean;
  status: StatusEnum;
}
