import {Point2D} from "../Utils/Point2D";
import {Player} from "../Entity/Player";
import {Rectangle} from "../Utils/Rectangle";
import {GameMap} from "../Entity/GameMap";

type CheckType = (destination: Point2D) => boolean;

/**
 * Handle collisions
 */
class CollisionsManager {
    private static tileMap: Map<number, CheckType> = new Map<number, CheckType>()
    /**
     * Decorator allowing tile registration
     * @param {number} tileId The block identifier
     */
    public static register(tileId: number) {
        return function command(target: any, name: string, descriptor: TypedPropertyDescriptor<CheckType>) {
            CollisionsManager.tileMap.set(tileId, <CheckType>descriptor.value);
        }
    }


    /**
     * Check if a point collide in a tile
     * @param {Point2D} destination Player's destination
     * @param {number} tileId Tile identifier
     */
    private static tileCollide(destination: Point2D, tileId: number) {
        const tileChecker = this.tileMap.get(tileId);
        if (tileChecker !== undefined) {
            return tileChecker(destination)
        }
        return false;
    }

    /**
     * Check if any side of the player collides
     * @param {GameMap} map The current map
     * @param {Player} player The player object
     * @param {Point2D} destination The player destination
     * @return {boolean}
     */
    public static collide(map: GameMap, player: Player, destination: Point2D): boolean {
        for (let point of new Rectangle(destination, player.width, player.height).getEdges()) {
            const tileId = map.getCollisionTile(point);
            const tilePosition = map.toTilePosition(point);
            if (this.tileCollide(tilePosition, tileId)) {
                return true;
            }
        }
        return false;
    }
}

export {CollisionsManager}