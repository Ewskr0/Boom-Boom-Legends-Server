import {Point2D} from "./Point2D";

class Rectangle {
    constructor(private origin: Point2D, private width: number, private height: number) {}

    public getEdges(): [Point2D, Point2D, Point2D, Point2D] {
        return [
            { x: this.origin.x, y: this.origin.y },
            { x: this.origin.x + this.width, y: this.origin.y },
            { x: this.origin.x + this.width, y: this.origin.y + this.height },
            { x: this.origin.x, y: this.origin.y + this.height }
        ];
    }

    public getCenter(): Point2D {
        return { x: this.origin.x + this.width / 2, y: this.origin.y + this.width / 2 }
    }
}

export { Rectangle }