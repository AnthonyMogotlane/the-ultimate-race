import { Component, OnDestroy, OnInit } from '@angular/core';
import { RaceService, RaceStatus, RacePosition } from '../race';
import { interval, Subscription } from 'rxjs';
import { DecimalPipe, DatePipe, CommonModule } from '@angular/common';

@Component({
  selector: 'app-race',
  imports: [CommonModule, DecimalPipe, DatePipe],
  templateUrl: './race.html',
  styleUrl: './race.css',
})

export class Race  implements OnInit, OnDestroy  {

  raceStatus: RaceStatus | null = null;
  positions: RacePosition[] = [];
  results: string[] = [];
  raceStarted = false;
  
  private statusSubscription: Subscription | null = null;
  private positionsSubscription: Subscription | null = null;

  constructor(private raceService: RaceService) {}

  ngOnInit() {
    // Subscribe to race started status
    this.raceService.getRaceStarted().subscribe((started: boolean) => {
      this.raceStarted = started;
      if (started) {
        this.startPolling();
      } else {
        this.stopPolling();
      }
    });

    // Get initial status
    this.raceService.getRaceStatus().subscribe(status => {
      this.raceStatus = status;
      this.raceService.setRaceStarted(status.raceStarted);
    });
  }

  startRace() {
    this.raceService.startRace().subscribe({
      next: (response: any) => {
        console.log('Race started:', response);
        this.raceService.setRaceStarted(true);
        
        // Load initial results
        this.raceService.getResults().subscribe((results: string[]) => {
          this.results = results;
        });
      },
      error: (error: any) => {
        console.error('Error starting race:', error);
      }
    });
  }

  startPolling() {
    // Start polling for status updates
    this.statusSubscription = this.raceService.pollRaceStatus().subscribe(
      (      status: any) => {
        this.raceStatus = status;
      },
      (      error: any) => console.error('Error polling status:', error)
    );

    // Start polling for position updates
    this.positionsSubscription = this.raceService.pollPositions().subscribe(
      (      positions: RacePosition[]) => {
        this.positions = positions;
      },
      (      error: any) => console.error('Error polling positions:', error)
    );

    // Poll results every 2 seconds
    interval(2000).subscribe(() => {
      this.raceService.getResults().subscribe((results: string[]) => {
        this.results = results;
      });
    });
  }

  stopPolling() {
    if (this.statusSubscription) {
      this.statusSubscription.unsubscribe();
    }
    if (this.positionsSubscription) {
      this.positionsSubscription.unsubscribe();
    }
  }

  getLeaderboard() {
    return this.positions.map((p, index) => ({
      ...p,
      rank: index + 1
    }));
  }

  getProgressPercentage(vehicle: string): number {
    const key = vehicle.toLowerCase() as keyof RaceStatus['positions'];
    if (!this.raceStatus || !this.raceStatus.distancesToWin[key]) {
      return 0;
    }
    const current = this.raceStatus.positions[key];
    const total = this.raceStatus.distancesToWin[key];
    return Math.min((current / total) * 100, 100);
  }

  getVehiclePosition(vehicle: string): number {
    const key = vehicle.toLowerCase() as keyof RaceStatus['positions'];
    return this.raceStatus?.positions[key] ?? 0;
  }

  getVehicleTarget(vehicle: string): number {
    const key = vehicle.toLowerCase() as keyof RaceStatus['distancesToWin'];
    return this.raceStatus?.distancesToWin[key] ?? 0;
  }

  getVehicleIcon(vehicle: string): string {
    const iconMap: { [key: string]: string } = {
      'chopper': '🚁',
      'bike': '🏍️',
      'tesla': '⚡',
      'nuclear sub': '🚢'
    };
    return iconMap[vehicle.toLowerCase()] || '🚗';
  }

  ngOnDestroy() {
    this.stopPolling();
  }


}
export { RaceService };

