import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, interval, BehaviorSubject } from 'rxjs';
import { startWith, switchMap } from 'rxjs/operators';

export interface RaceStatus {
  raceStarted: boolean;
  contestantsFinished: number;
  contestantsRemaining: number;
  positions: {
    chopper: number;
    bike: number;
    tesla: number;
    nuclearsub: number;
  };
  distancesToWin: {
    chopper: number;
    bike: number;
    tesla: number;
    nuclearsub: number;
  };
  lastUpdated: string;
}

export interface RacePosition {
  vehicle: string;
  position: number;
}

@Injectable({
  providedIn: 'root'
})
export class RaceService {
  private apiUrl = 'https://localhost:7258';
  private raceStarted = new BehaviorSubject<boolean>(false);

  constructor(private http: HttpClient) { }

  // Start the race
  startRace(): Observable<any> {
    return this.http.post(`${this.apiUrl}/race/start`, {});
  }

  // Get current race status
  getRaceStatus(): Observable<RaceStatus> {
    return this.http.get<RaceStatus>(`${this.apiUrl}/race/status`);
  }

  // Get current positions
  getPositions(): Observable<RacePosition[]> {
    return this.http.get<RacePosition[]>(`${this.apiUrl}/race/positions`);
  }

  // Get race results
  getResults(): Observable<string[]> {
    return this.http.get<string[]>(`${this.apiUrl}/race/results`);
  }

  // Poll for race status updates every second
  pollRaceStatus(): Observable<RaceStatus> {
    return interval(1000).pipe(
      startWith(0),
      switchMap(() => this.getRaceStatus())
    );
  }

  // Poll for positions updates
  pollPositions(): Observable<RacePosition[]> {
    return interval(1000).pipe(
      startWith(0),
      switchMap(() => this.getPositions())
    );
  }

  setRaceStarted(status: boolean) {
    this.raceStarted.next(status);
  }

  getRaceStarted(): Observable<boolean> {
    return this.raceStarted.asObservable();
  }
}
