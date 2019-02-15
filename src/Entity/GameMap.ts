import {Point2D} from "../Utils/Point2D";
import {Bomb} from "./Bomb";

class GameMap {
    private bombLayer: (Bomb|null)[];
    constructor(readonly width: number,
                readonly height: number,
                private collisionLayer: [number],
                private objectLayer: [number]) {
        this.bombLayer = new Array(width * height).fill(null);
    }

    /**
     * Get the tile containing the given position
     * @param position
     */
    public getCollisionTile(position: Point2D): number {
        const x = Math.trunc(position.x);
        const y = Math.trunc(position.y);
        return this.collisionLayer[y * this.width + x];
    }

    /**
     * Get the position inside the tile
     * @param position
     */
    public toTilePosition(position: Point2D): Point2D {
        return { x: position.x % 1, y: position.y % 1};
    }
}

export { GameMap }