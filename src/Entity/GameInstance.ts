import {GameMap} from "./GameMap";
import * as socketIo from "socket.io"


class GameInstance {
    private clients: socketIo.Socket[] = [];
    private running: boolean = false;

    constructor(private map: GameMap) {
    }

    public addClient(client: socketIo.Socket): boolean {
        if (this.running) {
            return false;
        }
        this.clients.push(client);
        return true;
    }

    public start() {
        // TODO: IMPLEMENT
        this.running = true;
    }
}

export { GameInstance }