export enum QueryStateEnum {
  Init = 'init',
  Loading = 'loading',
  Finished = 'finished',
  Error = 'error'
}

export interface ApiResponse {
  state: QueryStateEnum;
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


